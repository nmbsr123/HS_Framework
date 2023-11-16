using System.Collections.Generic;
using framework;

namespace game
{
    public class GameRuner : Singleton<GameRuner>
    {
        private List<IGameUpdate> _updateList = new List<IGameUpdate>();
        private List<IGameFixUpdate> _fixedUpdateList = new List<IGameFixUpdate>();
        private List<IGameLateUpdate> _lateUpdateList = new List<IGameLateUpdate>();

        public void RegisterUpdate(IGameUpdate update)
        {
            if (!_updateList.Contains(update))
            {
                _updateList.Add(update);
            }
        }
    
        public void RemoveUpdate(IGameUpdate update)
        {
            _updateList.Remove(update);
        }
        
        public void RegisterFixUpdate(IGameFixUpdate update)
        {
            if (!_fixedUpdateList.Contains(update))
            {
                _fixedUpdateList.Add(update);
            }
        }
    
        public void RemoveFixUpdate(IGameFixUpdate update)
        {
            _fixedUpdateList.Remove(update);
        }
        
        public void RegisterLateUpdate(IGameLateUpdate update)
        {
            if (!_lateUpdateList.Contains(update))
            {
                _lateUpdateList.Add(update);
            }
        }
    
        public void RemoveLateUpdate(IGameLateUpdate update)
        {
            _lateUpdateList.Remove(update);
        }
        
        public void Update(float deltaTime)
        {
            foreach (var data in _updateList)
            {
                data.Update(deltaTime);
            }
        }

        public void FixedUpdate()
        {
            foreach (var data in _fixedUpdateList)
            {
                data.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (var data in _lateUpdateList)
            {
                data.LateUpdate();
            }
        }
    }
}
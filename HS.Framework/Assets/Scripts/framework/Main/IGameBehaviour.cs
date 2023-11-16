namespace game
{
    public interface IGameUpdate
    {
        void Update(float deltaTime);
    }
    
    public interface IGameFixUpdate
    {
        void FixedUpdate();
    }

    public interface IGameLateUpdate
    {
        void LateUpdate();
    }
}
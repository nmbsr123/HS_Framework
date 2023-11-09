using framework.Log;
using UnityEngine;

namespace framework.UI
{
    public abstract class BasePresenter
    { 
        private View _view = null;
        private bool _isCache = false;
        
        public View View
        {
            get
            {
                if (_view == null)
                {
                    GameLog.Error("view is null");
                    return null;
                }

                return _view;
            }
            set => _view = value;
        }

        public T GetCom<T>(string nodeName) where T : Component
        {
            return View.GetCom<T>(nodeName);
        }
        public abstract void OnCreate();
        public abstract void RefreshUI();
        public abstract void InitData();
        public abstract void Register();
        public abstract void UnRegister();
        public abstract void OnDispose();
        public void Show()
        {
            InitData();
            RefreshUI();
            UnRegister();
            Register();
        }
        
        public void Dispose()
        {
            OnDispose();
            UnRegister();
            if (!_isCache)
            {
                if (_view.Handler != null)
                {
                    _view.Handler.Unload();
                }
            }
        }
    }
}
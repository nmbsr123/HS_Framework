using framework.Log;
using UnityEngine;

namespace framework.UI
{
    public abstract class BasePresenter
    { 
        private View _view = null;
        private bool _isCache = false;
        private UIConfig _uiConfig;
        
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
        
        public UIConfig UIConfig
        {
            get
            {
                if (_uiConfig == null)
                {
                    GameLog.Error("_uiConfig is null");
                    return null;
                }

                return _uiConfig;
            }
            set => _uiConfig = value;
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
        private object _uiParam = null;

        public bool IsActive()
        {
            if (_view == null)
            {
                return false;
            }

            return _view.IsActive();
        }

        public bool IsLastSlibingIndex()
        {
            if (_view == null)
            {
                return false;
            }
            return _view.IsLastSlibingIndex();
        }

        public void Active(bool bActive)
        {
            if (_view == null)
            {
                return;
            }
            _view.Active(bActive);
        }

        public void SetParam(object param)
        {
            _uiParam = param;
        }

        protected T GetParam<T>() where T : class
        {
            return _uiParam as T;
        }

        public void Show()
        {
            if (_view == null)
            {
                return;
            }
            _view.Active(false);
            InitData();
            RefreshUI();
            Register();
            _view.Active(true);
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
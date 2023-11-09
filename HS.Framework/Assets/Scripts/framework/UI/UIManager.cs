using System.Collections.Generic;
using framework.Resource;

namespace framework.UI
{
    public enum ViewType
    {
        Null = 0,
        Panel,
        Dialog,
        Subview,
        Guide,
        System,
    }
    
    public class UIConfig
    {
        public ViewType ViewType = ViewType.Null;
        public string Path = string.Empty;
        public bool isLobby = false;
    }
    
    public class UIManager : BaseManager
    {
        private Stack<int> _stackOperation = new Stack<int>();
        private Dictionary<int, LoaderHandler> _dicLoaderHandlers = new Dictionary<int, LoaderHandler>();

        public override void Init()
        {
            base.Init();
            
        }
        
        public override void Dispose()
        {
            base.Init();
            
        }

        private void LoadSync(int uiid, UIConfig uiConfig)
        {
            LoaderHandler handler = null;
            if (_dicLoaderHandlers.TryGetValue(uiid, out handler))
            {
                
            }
            else
            {
                handler = new LoaderHandler();
                _dicLoaderHandlers.Add(uiid, handler);
            }
        }
    }
}
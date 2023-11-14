using System.Collections.Generic;
using framework.Log;
using framework.Resource;
using UnityEditor;
using UnityEngine;

namespace framework.UI
{
    public enum ViewType
    {
        Null = 0,
        Panel,
        Dialog,
        Message,
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
    
    
    public class UIManager : Singleton<UIManager>, IManager 
    {
        private Stack<int> _stackOperation = new Stack<int>();
        private Dictionary<int, ViewLoaderHander> _dicLoaderHandlers = new Dictionary<int, ViewLoaderHander>();
        private Dictionary<int, MainViewPresenter> _dicMainPresenters = new Dictionary<int, MainViewPresenter>();
        private Dictionary<ViewType, Transform> _dicTransforms = new Dictionary<ViewType, Transform>();
        private Transform _uiroot = null;
        private readonly Dictionary<ViewType, int> _dicViewToLayerOrder = new Dictionary<ViewType, int>()
        {
            [ViewType.Panel] = 1000,
            [ViewType.Dialog] = 1200,
            [ViewType.Message] = 1400,
            [ViewType.Guide] = 1600,
            [ViewType.System] = 1800,
        };

        private const int DELTA_LAYER = 10;

        private const string _uiRootPath = "UIRoot";
        
        protected override void OnCreate()
        {
        }
        
        public void Init()
        {
            if (_uiroot == null)
            {
                _uiroot = ResourceManager.Instance.EditLoad<GameObject>(_uiRootPath).transform;
                GameObject.DontDestroyOnLoad(_uiroot);
                _dicTransforms.Add(ViewType.Panel, _uiroot.Find("Canvas/PanelView"));
                _dicTransforms.Add(ViewType.Dialog, _uiroot.Find("Canvas/DialogView"));
                _dicTransforms.Add(ViewType.Message, _uiroot.Find("Canvas/MessageView"));
                _dicTransforms.Add(ViewType.Guide, _uiroot.Find("Canvas/GuideView"));
                _dicTransforms.Add(ViewType.System, _uiroot.Find("Canvas/SystemView"));
            }
        }
        
        public void Dispose()
        {
            
        }

        private void ShowMain<T>(int uiid, UIConfig uiConfig) where T : MainViewPresenter
        {
            if (_dicMainPresenters.TryGetValue(uiid, out var mainViewPresenter))
            {
                //todo 判读界面栈逻辑
                int curTopLayer = GetCurTopLayer(uiConfig.ViewType) + DELTA_LAYER;
                //如果已经显示
                if (mainViewPresenter.IsActive())
                {
                    //是否是最高层级
                    if (mainViewPresenter.IsLastSlibingIndex())
                    {
                        return;
                    }
                    //移动到最高层
                    mainViewPresenter.View.SetCanvasOrder(curTopLayer);
                    _stackOperation.Push(uiid);
                    return;
                }
                
            }
            else
            {
                handler = new ViewLoaderHander();
                _dicLoaderHandlers.Add(uiid, handler);
                
            }
        }

        /// <summary>
        /// 获得某个界面类型当前最高layer
        /// </summary>
        /// <param name="viewType">界面类型</param>
        /// <returns></returns>
        private int GetCurTopLayer(ViewType viewType)
        {
            if (!_dicTransforms.ContainsKey(viewType))
            {
                GameLog.Error($"没有该类型 {viewType}");
                return 0;
            }

            var trs = _dicTransforms[viewType];
            if (trs.childCount == 0)
            {
                return _dicViewToLayerOrder[viewType];
            }

            var canvas = trs.GetChild(trs.childCount - 1).GetComponent<Canvas>();
            if (canvas == null)
            {
                GameLog.Error("找不到canvas");
                return 0;
            }

            return canvas.sortingOrder;
        }
    }
}
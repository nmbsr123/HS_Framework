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
        public int uiID = 0;
        public ViewType viewType = ViewType.Null;
        public string path = string.Empty;
        public bool isLobby = false;
        public bool isCache = false;
    }
    
    
    public class UIManager : Singleton<UIManager>, IManager 
    {
        private Stack<int> _stackOperation = new Stack<int>();
        private Dictionary<int, ViewLoaderHander> _dicLoaderHandlers = new Dictionary<int, ViewLoaderHander>();
        private Dictionary<int, MainViewPresenter> _dicMainPresenters = new Dictionary<int, MainViewPresenter>();
        private Dictionary<ViewType, Transform> _dicTransforms = new Dictionary<ViewType, Transform>();
        private Transform _uiroot = null;
        private bool _isInStackView = false; //是否处于界面栈
        private Stack<int> _stackViewCache = new Stack<int>();
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

        public void ShowMainPresenter<T>(int uiid, UIConfig uiConfig, BaseUIParam uiParam = null, bool isAsync = false) where T : MainViewPresenter, new()
        {
            if (_dicMainPresenters.TryGetValue(uiid, out var mainViewPresenter))
            {
                CheckAndStartStackView(uiid, uiConfig);
                int curTopLayer = GetCurTopLayer(uiConfig.viewType) + DELTA_LAYER;
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
                mainViewPresenter.View.SetCanvasOrder(curTopLayer);
                mainViewPresenter.Show();
                _stackOperation.Push(uiid);
            }
            else
            {
                
                if (_dicLoaderHandlers.TryGetValue(uiid, out var handler))
                {
                    //说明界面正在打开中
                    return;
                }
                else
                {
                    CheckAndStartStackView(uiid, uiConfig);
                    T presenter = new T();
                    if (isAsync)
                    {
                        handler = ResourceManager.Instance.LoadAsync<GameObject>(uiConfig.path, delegate(Object o)
                        {
                            
                            InitAndShowPanel(presenter, uiConfig, GameObject.Instantiate(o) as GameObject, uiParam);
                        }) as ViewLoaderHander;
                    }
                    else
                    {
                        handler = ResourceManager.Instance.LoadSync<GameObject>(uiConfig.path) as ViewLoaderHander;
                        InitAndShowPanel(presenter, uiConfig, GameObject.Instantiate(handler.gameObject), uiParam);
                    }
                    _dicLoaderHandlers.Add(uiid, handler);
                    _stackOperation.Push(uiid);
                }
            }
        }

        /// <summary>
        /// 界面栈只会处理PanelPresenter(一级界面)和DialogPresenter(二级界面)
        /// </summary>
        /// <param name="uiid"></param>
        /// <param name="uiConfig"></param>
        /// <returns></returns>
        private bool CheckAndStartStackView(int uiid, UIConfig uiConfig)
        {
            if (_stackOperation.Count == 0)
            {
                return false;
            }

            var lastUIID = _stackOperation.Peek();
            if (_dicMainPresenters.TryGetValue(lastUIID, out var lastPresenter))
            {
                if (lastPresenter.UIConfig.viewType == ViewType.Dialog && uiConfig.viewType == ViewType.Panel)
                {
                    if (_isInStackView)
                    {
                        DisposeStackView();
                    }

                    _isInStackView = true;
                    if (_stackViewCache.Count > 0)
                    {
                        GameLog.Error("CheckAndStartStackView error");
                        return false;
                    }
                    _stackViewCache.Clear();
                    //将当前显示的界面入栈并隐藏
                    foreach (var presenter in _dicMainPresenters.Values)
                    {
                        presenter.Active(false);
                        _stackViewCache.Push(presenter.UIConfig.uiID);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //理论上不会走到这
                GameLog.Error("CheckAndStartStackView error");
                return false;
            }
        }

        private void DisposeStackView()
        {
            while (_stackViewCache.Count > 0)
            {
                var uiid = _stackViewCache.Pop();
                if (_dicMainPresenters.TryGetValue(uiid, out var presenter))
                {
                    if (presenter.IsLobby())
                    {
                        presenter.Active(true);
                    }
                    else
                    {
                        DisposePresenterByUIID(presenter.UIConfig);
                    }
                }
                else
                {
                    GameLog.Error("DisposeStackView error");
                }
            }
        }
        
        private void InitAndShowPanel(BasePresenter presenter, UIConfig uiConfig, GameObject obj, BaseUIParam uiParam = null)
        {
            View view = new View();
            view.Handler = _dicLoaderHandlers[uiConfig.uiID]; //一定取得到
            view.UIRoot = obj;
            presenter.View = view;
            presenter.UIConfig = uiConfig;
            presenter.Show();
        }

        private void DisposePresenterByUIID(UIConfig uiConfig)
        {
            BasePresenter prensenter = GetExistPresenter(uiConfig);
            if (prensenter == null)
            {
                GameLog.Error("DisposePresenterByUIID error1");
                return;
            }
            prensenter.Active(false);
            if (uiConfig.isCache)
            {
                return;
            }
            prensenter.Dispose();
            RemoveExistPresenter(uiConfig);
        }

        private BasePresenter GetExistPresenter(UIConfig uiConfig)
        {
            switch (uiConfig.viewType)
            {
                case ViewType.Panel:
                
                case ViewType.Dialog:
                {
                    if (_dicMainPresenters.TryGetValue(uiConfig.uiID, out var presenter))
                    {
                        return presenter;
                    }
                    else
                    {
                        return null;
                    }
                }
                case ViewType.Message:
                {

                    return null;
                }
                case ViewType.Guide:
                {
                    return null;
                }
                case ViewType.System:
                {
                    return null;
                }
                default:
                {
                    return null;
                }
            }
        }

        private void RemoveExistPresenter(UIConfig uiConfig)
        {
            switch (uiConfig.viewType)
            {
                case ViewType.Panel:
                
                case ViewType.Dialog:
                {
                    _dicMainPresenters.Remove(uiConfig.uiID);
                    return;
                }
                case ViewType.Message:
                {

                    return;
                }
                case ViewType.Guide:
                {
                    return;
                }
                case ViewType.System:
                {
                    return;
                }
                default:
                {
                    return;
                }
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
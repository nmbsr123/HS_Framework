using System;
using System.Collections.Generic;
using framework.Log;
using framework.Resource;
using UnityEngine;
namespace framework.UI
{
    public class View : IView
    {
        private LoaderHandler _handler = null;
        private GameObject _uiRoot = null;
        private Dictionary<string, NodeBind> _dicNodeBind = new Dictionary<string, NodeBind>();

        public LoaderHandler Handler
        {
            get
            {
                if (_handler == null)
                {
                    GameLog.Error("_handler is null");
                    return null;
                }

                return _handler;
            }
            set => _handler = value;
        }
        
        public GameObject UIRoot
        {
            get
            {
                if (_uiRoot == null)
                {
                    GameLog.Error("_uiRoot is null");
                    return null;
                }

                return _uiRoot;
            }
            set => _uiRoot = value;
        }
        
        public void InitNodeBind()
        {
            if (_uiRoot == null)
            {
                GameLog.Error("_handler's asset is null");
                return;
            }
            var coms = _uiRoot.GetComponentsInChildren<NodeBind>();
            foreach (var com in coms)
            {
                _dicNodeBind.Add(com.name, com);
            }
        }

        public void SetCanvasOrder(int order)
        {
            if (_uiRoot == null)
            {
                return;
            }

            var canvas = _uiRoot.GetComponent<Canvas>();
            if (canvas == null)
            {
                GameLog.Error("canvas is null");
                return;
            }

            canvas.sortingOrder = order;
        }

        public T GetCom<T>(string nodeName) where T : Component
        {
            if (_dicNodeBind.ContainsKey(nodeName))
            {
                return _dicNodeBind[nodeName].GetCom<T>();
            }
            else
            {
                GameLog.Error("node not exits : " + nodeName);
                return null;
            }
        }

        public bool IsActive()
        {
            if (_uiRoot == null)
            {
                return false;
            }

            return _uiRoot.activeSelf;
        }

        public void Active(bool bActive)
        {
            if (_uiRoot == null)
            {
                return;
            }

            if (_uiRoot.activeSelf == bActive)
            {
                return;
            }
            _uiRoot.SetActive(bActive);
        }

        public bool IsLastSlibingIndex()
        {
            if (_uiRoot == null)
            {
                return false;
            }
            return _uiRoot.transform.GetSiblingIndex() == _uiRoot.transform.parent.childCount - 1;
        }
    }
}
using System;
using System.Collections.Generic;
using framework.Log;
using framework.Resource;
using UnityEngine;
namespace framework.UI
{
    public class View : IView
    {
        private ViewLoaderHander _handler = null;
        private Dictionary<string, NodeBind> _dicNodeBind = new Dictionary<string, NodeBind>();

        public ViewLoaderHander Handler
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
        
        public void InitNodeBind()
        {
            if (_handler.gameObject == null)
            {
                GameLog.Error("_handler's asset is null");
                return;
            }
            var coms = _handler.gameObject.GetComponentsInChildren<NodeBind>();
            foreach (var com in coms)
            {
                _dicNodeBind.Add(com.name, com);
            }
        }

        public void SetCanvasOrder(int order)
        {
            if (_handler == null || _handler.gameObject == null)
            {
                return;
            }

            var canvas = _handler.gameObject.GetComponent<Canvas>();
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
            if (_handler == null)
            {
                return false;
            }

            if (_handler.gameObject == null)
            {
                return false;
            }

            return _handler.gameObject.activeSelf;
        }

        public void Active(bool bActive)
        {
            if (_handler == null || _handler.gameObject == null)
            {
                return;
            }

            if (_handler.gameObject.activeSelf == bActive)
            {
                return;
            }
            _handler.gameObject.SetActive(bActive);
        }

        public bool IsLastSlibingIndex()
        {
            if (_handler == null || _handler.gameObject == null)
            {
                return false;
            }
            return _handler.gameObject.transform.GetSiblingIndex() == _handler.gameObject.transform.parent.childCount - 1;
        }
    }
}
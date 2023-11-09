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
        
        public void InitNodeBind()
        {
            if (_handler.asset == null)
            {
                GameLog.Error("_handler's asset is null");
                return;
            }
            var coms = (_handler.asset as GameObject).GetComponentsInChildren<NodeBind>();
            foreach (var com in coms)
            {
                _dicNodeBind.Add(com.name, com);
            }
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
    }
}
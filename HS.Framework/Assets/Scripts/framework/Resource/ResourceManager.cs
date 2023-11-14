using System;
using UnityEngine;

namespace framework.Resource
{
    public class ResourceManager : Singleton<ResourceManager>, IManager 
    {
        private BaseLoadStrategy _loadStrategy = null;
        protected override void OnCreate()
        {
            throw new NotImplementedException();
        }
        
        public void Init()
        {
            if (true)
            {
                _loadStrategy = new AssetBundleLoadStrategy();
            }
        }

        public LoaderHandler LoadSync<T>(string path) where T : UnityEngine.Object
        {
            return _loadStrategy.LoadSync<T>(path);
        }
        
        public LoaderHandler LoadAsync<T>(string path, Action<UnityEngine.Object> onComplete) where T : UnityEngine.Object
        {
            return _loadStrategy.LoadAync<T>(path, onComplete);
        }

        public T EditLoad<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        public void Dispose()
        {
            _loadStrategy.Dispose();
        }
    }
}
using System;
using UnityEngine;

namespace framework.Resource
{
    public class ResourceManager : Singleton<ResourceManager>, IManager 
    {
        private BaseLoadStrategy _loadStrategy = null;
        public void Init()
        {
            if (true)
            {
                _loadStrategy = new AssetBundleLoadStrategy();
            }
        }

        public LoaderHandler LoadPrefabSync<T>(string path) where T : UnityEngine.Object
        {
            return _loadStrategy.LoadPrefabSync<T>(path);
        }
        
        public LoaderHandler LoadPrefabAsync<T>(string path, Action<UnityEngine.Object> onComplete) where T : UnityEngine.Object
        {
            return _loadStrategy.LoadPrefabAync<T>(path, onComplete);
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
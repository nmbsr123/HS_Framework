using System;

namespace framework.Resource
{
    public class ResourceManager : BaseManager
    {
        private BaseLoadStrategy _loadStrategy = null;
        public override void Init()
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

        public override void Dispose()
        {
            _loadStrategy.Dispose();
        }
    }
}
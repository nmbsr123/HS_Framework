using System;
using System.Linq;

namespace framework.Resource
{
    public class AssetBundleLoadStrategy : BaseLoadStrategy
    {
        private AssetBundleManager mAssetBundleManager => AssetBundleManager.Instance;
        public AssetBundleLoadStrategy()
        {
            AssetBundleManager.CreateInstance();
            mAssetBundleManager.Init();
        }

        public override LoaderHandler LoadPrefabSync<T>(string path)
        {
            var bundleName = ParsePath(path);
            BundleEntity bundleEntity = mAssetBundleManager.LoadBundleEntitySync(bundleName);
            var asset = bundleEntity.AbBundle.LoadAsset<T>(path + ".prefab");
            return new LoaderHandler()
            {
                loadStrategy = this,
                bundleEntity = bundleEntity,
                asset = asset
            };
        }

        public override LoaderHandler LoadPrefabAync<T>(string path, Action<UnityEngine.Object> onComplete)
        {
            var bundleName = ParsePath(path);
            LoaderHandler loaderHandler = new LoaderHandler();
            loaderHandler.loadStrategy = this;
            mAssetBundleManager.LoadBundleEntityAsync(bundleName, entity =>
            {
                loaderHandler.bundleEntity = entity;
                loaderHandler.asset = entity.AbBundle.LoadAsset<T>(path + ".prefab");
                onComplete?.Invoke(loaderHandler.asset);
            });
            return loaderHandler;
        }

        /// <summary>
        /// 根据path解析，返回bundleName
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ParsePath(string path)
        {
            path = path.ToLower();
            int targetIndex = 0;
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '/')
                {
                    targetIndex = i;
                    break;
                }
            }
            if (path.Contains("allinone"))
            {
                return path.Substring(0, targetIndex) + ".unity3d";
            }
            else
            {
                return path + ".unity3d";
            }
        }
        
        public override void Unload(string bundleName)
        {
            mAssetBundleManager.Unload(bundleName);
        }

        public override void Dispose()
        {
            mAssetBundleManager.Dispose();
        }
    }
}
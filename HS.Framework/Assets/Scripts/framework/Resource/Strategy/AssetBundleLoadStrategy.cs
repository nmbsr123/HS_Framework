using System;
using System.Linq;

namespace framework.Resource
{
    public class AssetBundleLoadStrategy : BaseLoadStrategy
    {
        private AssetBundleMgr _assetBundleMgr = null;
        public AssetBundleLoadStrategy()
        {
            _assetBundleMgr = new AssetBundleMgr();
            _assetBundleMgr.InitDependConfig();
        }

        public override LoaderHandler LoadSync<T>(string path)
        {
            BundleEntity bundleEntity = _assetBundleMgr.LoadBundleEntitySync(path);
            string assetName = string.Empty;
            var strArray = path.Split('/');
            assetName = strArray.Last();
            return new LoaderHandler()
            {
                loadStrategy = this,
                bundleEntity = bundleEntity,
                asset = bundleEntity.AbBundle.LoadAsset<T>(assetName)
            };
        }

        public override LoaderHandler LoadAync<T>(string path, Action<UnityEngine.Object> onComplete)
        {
            LoaderHandler loaderHandler = new LoaderHandler();
            loaderHandler.loadStrategy = this;
            string assetName = string.Empty;
            var strArray = path.Split('/');
            assetName = strArray.Last();
            _assetBundleMgr.LoadBundleEntityAsync(path, entity =>
            {
                loaderHandler.bundleEntity = entity;
                loaderHandler.asset = entity.AbBundle.LoadAsset<T>(assetName);
            });
            return loaderHandler;
        }

        public override void Unload(string bundleName)
        {
            _assetBundleMgr.Unload(bundleName);
        }

        public override void Dispose()
        {
            _assetBundleMgr.Dispose();
        }
    }
}
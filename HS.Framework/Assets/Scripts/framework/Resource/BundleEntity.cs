using System;
using System.Collections.Generic;
using UnityEngine;

namespace framework.Resource
{
    public enum EventType {
        OnBundleLoaded,
    }
    
    public class BundleEntity
    {
        private string _bundleName = string.Empty; //资产名字
        private int mRefCount = 0; //引用计数
        private int mDependCompletedCount = 0; //依赖项加载完成的个数
        private string[] mDependArray = null; //依赖项
        private AssetBundle mAbBundle = null;
        private List<Action<BundleEntity>> mLoadedCallbackList = new List<Action<BundleEntity>>(); //异步回调，只会在异步加载AB才会用到

        public string BundleName => _bundleName; //资产名字
        public int RefCount => mRefCount; //引用计数
        public AssetBundle AbBundle => mAbBundle;
        public BundleEntity(string assetName, string[] dependArray, bool isAsync = false)
        {
            _bundleName = assetName;
            mDependArray = dependArray;
            if (isAsync)
            {
                EventUtil.AddListener(EventType.OnBundleLoaded, OnBundleLoaded);
            }
        }

        private void OnBundleLoaded(object bundleEntity)
        {
            for (int i = 0; i < mDependArray.Length; i++)
            {
                var entity = bundleEntity as BundleEntity;
                if (entity._bundleName == mDependArray[i]) //如果自己的依赖项完成加载
                {
                    mDependCompletedCount++;
                }
            }

            if (mDependCompletedCount == mDependArray.Length)
            {
                EventUtil.RemoveListener(EventType.OnBundleLoaded, OnBundleLoaded);
            }
        }

        public void AddRefCount()
        {
            mRefCount++;
        }
        
        public void ReduceRefCount()
        {
            mRefCount--;
        }

        public bool NeedToDestroy()
        {
            return mRefCount == 0;
        }

        public void Dispose()
        {
            if (mRefCount == 0 && mAbBundle != null)
            {
                mAbBundle.Unload(true);
            }
            EventUtil.RemoveListener(EventType.OnBundleLoaded, OnBundleLoaded);
        }
        
        public void ForthDispose()
        {
            if (mAbBundle != null)
            {
                mAbBundle.Unload(true);
            }

            mRefCount = 0;
            EventUtil.RemoveListener(EventType.OnBundleLoaded, OnBundleLoaded);
        }

        public void SetAssetBundle(AssetBundle assetBundle)
        {
            mAbBundle = assetBundle;
        }
        
        public void AddLoadCallback(Action<BundleEntity> func)
        {
            mLoadedCallbackList.Add(func);
        }

        //如果该bundle的依赖bundle全部加载完成，那就执行回调队列
        public void DoCallback()
        {
            if (mAbBundle == null)
            {
                return;
            }
            if (mLoadedCallbackList.Count == 0)
            {
                return;
            }
            foreach (var callback in mLoadedCallbackList)
            {
                callback?.Invoke(this);
            }
            mLoadedCallbackList.Clear();
        }

        public bool IsAllDependLoaded()
        {
            return mDependCompletedCount >= mDependArray.Length;
        }
    }
}
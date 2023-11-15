using System;

namespace framework.Resource
{
    public abstract class BaseLoadStrategy
    {
        public abstract LoaderHandler LoadPrefabSync<T>(string path) where T : UnityEngine.Object;
        public abstract LoaderHandler LoadPrefabAync<T>(string path, Action<UnityEngine.Object> onComplete) where T : UnityEngine.Object;
        public abstract void Unload(string path);
        public abstract void Dispose();
    }
}
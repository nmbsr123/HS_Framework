using System;

namespace framework.Resource
{
    public abstract class BaseLoadStrategy
    {
        public abstract LoaderHandler LoadSync<T>(string path) where T : UnityEngine.Object;
        public abstract LoaderHandler LoadAync<T>(string path, Action<UnityEngine.Object> onComplete) where T : UnityEngine.Object;
        public abstract void Unload(string path);
        public abstract void Dispose();
    }
}
namespace framework
{
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance = default(T);
        public static T Instance => _instance;

        protected abstract void OnCreate();
        
        public void CreateInstance()
        {
            _instance = new T();
            OnCreate();
        }
    }
}
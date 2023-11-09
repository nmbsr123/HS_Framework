namespace framework
{
    public  class BaseManager : Singleton<BaseManager>, IManager
    {
        protected bool _isInit;
        protected override void OnCreate()
        {
            Init();
            _isInit = true;
        }
        
        public virtual void Init()
        {
            
        }

        public virtual void Dispose()
        {
            
        }
    }
}
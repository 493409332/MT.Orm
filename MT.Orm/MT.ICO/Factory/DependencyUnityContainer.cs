using Microsoft.Practices.Unity;

namespace MT.ICO.Factory
{
    //创建一个容器的单例
    public static class DependencyUnityContainer
    {
        private static IUnityContainer _current;
        private static readonly object Locker = new object();
        public static IUnityContainer Current
        {
            get
            {
                if ( _current == null )
                {
                    lock ( Locker )
                    {
                        if ( _current == null )
                        {
                            _current = new UnityContainer();
                        }
                    }
                }
                return _current;
            }
        }
    }
}

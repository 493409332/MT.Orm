using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace MT.ICO.Factory
{

    //ICO API注册
    //GlobalConfiguration.Configuration.DependencyResolver = new IoCContainer(DependencyUnityContainer.Current);

    /// <summary>
    /// WebApi 接口注册方式
    /// </summary>
    public class IoCContainer : System.Web.Http.Dependencies.IDependencyResolver
    {

        IUnityContainer container;
        public IoCContainer()
        {
            this.container = DependencyUnityContainer.Current;
        }
        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return new ScopeContainer(container);
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch
            {
                return null;
            } 
          
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch
            {
                return null;
            }

        }

        public void Dispose()
        {
            container.Dispose();
        }
    }

    public class ScopeContainer : System.Web.Http.Dependencies.IDependencyScope
    {
        protected IUnityContainer container;
        public ScopeContainer(IUnityContainer container)
        {
            if ( container == null )
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }

        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch
            {
                return null;
            }

        }

        public void Dispose()
        {
            container.Dispose();
        }
    }

}

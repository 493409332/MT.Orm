using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MT.ICO.Factory
{


    //MVC 接口注册方式 
    // DependencyResolver.SetResolver(new UnityDependencyResolver());
    public class UnityDependencyResolver : IDependencyResolver
    {
        IUnityContainer container;
        public UnityDependencyResolver()
        {
            this.container = DependencyUnityContainer.Current;
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
                return new List<object>();
            }
        }
    }
}

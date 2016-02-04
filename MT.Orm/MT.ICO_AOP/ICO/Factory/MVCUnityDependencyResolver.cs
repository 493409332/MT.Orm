using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MT.ICO_AOP.ICO.Factory
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

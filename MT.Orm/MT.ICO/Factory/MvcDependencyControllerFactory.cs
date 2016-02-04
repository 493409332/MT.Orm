using Microsoft.Practices.Unity;
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace MT.ICO.Factory
{
    // RegisterDependency就是注册接口与实例的关系．
    // setCongrollerFactory则是用MyDependencyMvcControllerFactory替代默认Controller工厂 
    // DependencyFactory.RegisterDependency();

    //MVC 工厂注册方式 
    // ControllerBuilder.Current.SetControllerFactory(new MvcDependencyControllerFactory());

    public class MvcDependencyControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if ( controllerType == null )
            {
                throw new HttpException(404,
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "没有找到路由{0}",
                        requestContext.HttpContext.Request.Path));
            }
            if ( !typeof(IController).IsAssignableFrom(controllerType) )
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "{0}该类型没有继承ControllerBase",
                        controllerType),
                    "controllerType");
            }
            try
            {

                return (IController) DependencyUnityContainer.Current.Resolve(controllerType);
            }
            catch ( Exception ex )
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "{0}创建该controller失败！",
                        controllerType),
                    ex);
            }
        }
    }
}

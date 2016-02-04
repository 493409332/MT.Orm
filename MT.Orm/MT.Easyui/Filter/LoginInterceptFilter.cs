using Complex.Logical.Admin;
using MT.ICO.Factory;
using System.Web.Mvc;
namespace MT.Easyui
{

    public class LoginInterceptFilter : IActionFilter
    {
        #region IActionFilter 成员

        public IUserInfo iuserinfo
        {
            get
            {
                return (IUserInfo) DependencyUnityContainer.Current.Resolve(typeof(IUserInfo), "RUserInfo"); 
            }
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Request = filterContext.HttpContext.Request;
            var Session = filterContext.HttpContext.Session;
            var Cookies = filterContext.HttpContext.Request.Cookies;

            var Url = Request.Url.AbsolutePath;
            
            if ( !Url.StartsWith("/Admin/AdminLogin") )
            {
                if ( Cookies["user"] != null && Session["userinfo"] == null )
                {
                     iuserinfo.GetUserInfoBySession();
                }
                if ( Session["userinfo"] == null && Cookies["user"] == null )
                {
                    filterContext.Result = new RedirectResult("/Admin/AdminLogin", false);
                }
            }
           
        }


        //if (Session["userinfo"] != null) 
        //filterContext.HttpContext.Request.Url
    }

        #endregion

}

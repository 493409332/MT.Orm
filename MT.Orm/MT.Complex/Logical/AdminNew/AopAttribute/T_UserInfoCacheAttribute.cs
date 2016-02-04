using Complex.Logical;
using MT.AOP.Context;
using MT.Complex.Logical.Admin.CacheManag;
using System.Linq;
using System.Web;

namespace MT.Complex.Logical.Admin.AopAttribute
{
    public class T_UserInfoCacheAttribute : BaseEndAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        { 
            if (new string[] { "RUser", "RRole", "RNavigation", "RNavButtons" }.Contains(context.ClassFullName.Split('.').Last()) )
            {
                CacheManagement.Instance.RemoveStartsWith("T_UserInfo");
                var Session = HttpContext.Current.Session;
                Session["userinfo"] = null;
            } 
            return context;
        }
    }
}

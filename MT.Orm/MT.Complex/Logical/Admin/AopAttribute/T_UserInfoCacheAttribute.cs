using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Complex.Logical.Admin.CacheManag;
using MT.ICO_AOP.AOP.Context;
 

namespace Complex.Logical.Admin.AopAttribute
{
    public class T_UserInfoCacheAttribute : EndAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        { 
           context= base.Action(context);
           if ( new string[] { "RUser", "RRole", "RNavigation", "RNavButtons" }.Contains(context.ClassFullName.Split('.').Last()) && new string[] { "Add", "Edit", "Remove", "TrueRemove", "SetUseConfigrByKey", "setButtons" }.Contains(context.MethodName)   )
           {
               CacheManagement.Instance.RemoveStartsWith("T_UserInfo");
               var Session = HttpContext.Current.Session;
               Session["userinfo"] = null;
           }

            return context;
        }
    }
}

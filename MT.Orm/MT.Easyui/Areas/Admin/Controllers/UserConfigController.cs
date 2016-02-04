using Complex.Logical.Admin;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
namespace MT.Easyui.Areas.Admin.Controllers
{
    public class UserConfigController : BaseController
    {
        //
        // GET: /Admin/UserConfig/
        [Dependency("RUser")]
        public IUser iUser { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public JavaScriptResult GetConfig()
        {
            return  JavaScript( " var sys_config = " + iUser.GetConfig(CurrentUserID));
        }
        public int SetConfig(string json)
        {  
            return iUser.SetUseConfigrByKey(CurrentUserID, json);
        }
    }
}

using System.Web.Mvc;
 

namespace MT.Easyui
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginInterceptFilter());
        }
    }
}
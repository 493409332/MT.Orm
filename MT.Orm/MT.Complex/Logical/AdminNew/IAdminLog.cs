using MT.ICO_AOP.ICO.Attribute;
using MT.ICO_AOP.ICO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface IAdminLog : ITransientLifetimeManagerRegister
    {
        List<Log> GetPage(string predicate, int page, int page_size, string order, string asc, string runclass, string username, out int total);
         bool Delete();
         object GetClassList();
         object GetUserNamelist();
         
    }
}

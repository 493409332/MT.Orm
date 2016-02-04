using Complex.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using MT.Orm;
using MT.ICO.Attribute;
namespace Complex.Logical.Admin.Realization
{
    [ICOConfig("RRole")]
    public class RROLE : RBase<T_Role>, IRole
    {
        public RROLE()
        {

        }

     
 
        public List<T_Role> GetAllRole() {

            return GetAll().Where(p=>p.IsDelete ==false).ToList();
        }
 
 
    }
}
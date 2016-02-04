using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System.Collections.Generic;

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface IRole : ITransientLifetimeManagerRegister, IBase<T_Role>
    {
     
        List<T_Role> GetAllRole();
    }
}

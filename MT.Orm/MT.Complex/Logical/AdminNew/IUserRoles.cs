using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System.Collections.Generic;
 

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface IUserRoles : ITransientLifetimeManagerRegister, IBase<T_UserRoles>
    {
        List<T_UserRoles> GetRoleByUserID(int UserID);
     
        int AddUserTo(int UserID,int[] roleids);
    }
}

using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System.Collections.Generic;

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface IUser : ITransientLifetimeManagerRegister,IBase<T_User>
    {
        List<T_UserTem> GetAllUser();
        T_User GetUserByKey(int UserID);
        T_User GetByKey(int UserID);
        int SetUseConfigrByKey(int UserID, string json);
        string GetConfig(int UserID); 
    }
}

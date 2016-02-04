using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;

namespace Complex.Logical.Admin
{ 
    [ICO_AOPEnable(true)]
    public interface IUserInfo : ITransientLifetimeManagerRegister 
    {
        T_UserInfo GetUserInfo(T_User User);
        T_UserInfo GetUserInfoByID(int ID);
        T_UserInfo GetUserInfoBySession();
    }
}

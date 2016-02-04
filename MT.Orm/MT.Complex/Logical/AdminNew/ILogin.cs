using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface ILogin : ITransientLifetimeManagerRegister
    {
        bool SearchForName(string username);
        T_User SearchForNameorPwd(string username, string userpwd);
    }
     
}

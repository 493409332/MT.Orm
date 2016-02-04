using Complex.Entity.Admin;
using MT.ICO.Interface;
using System.Collections.Generic;


namespace Complex.Logical.Admin

{
    public interface IRoleNavBtns : ITransientLifetimeManagerRegister, IBase<T_RoleNavBtns>
    {
        List<T_RoleNavBtns> GetRoleNavBtns();
        List<T_RoleNavBtns> GetRoleNavBtnsByRole(int roleID);
        bool setRoleButtons(string Data);

    }
}

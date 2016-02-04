using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System.Collections.Generic;

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface INavigation : ITransientLifetimeManagerRegister, IBase<T_Navigation>
    {
        List<T_Navigation> GetTreeGrid(int ParentID);
        object GetNavigation();
        object GetNavigation(List<T_Navigation> list);
    }
}

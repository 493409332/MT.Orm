using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System.Collections.Generic;

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface IButton : ITransientLifetimeManagerRegister, IBase<T_Button>
    {
        List<T_Button> GetButtons();
    }
}

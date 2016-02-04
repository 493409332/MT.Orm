using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System.Collections.Generic;

namespace Complex.Logical.Admin
{ 
    [ICO_AOPEnable(true)]
    public interface INavButtons : ITransientLifetimeManagerRegister, IBase<T_NavButtons>
    {
        List<T_NavButtons> GetButByNavID(int NavID);
        bool setButtons(int NavID, int[] btns);
    }
}

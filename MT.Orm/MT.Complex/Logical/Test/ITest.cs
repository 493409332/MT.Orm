using MT.ICO.Attribute;
using MT.ICO.Interface;

namespace MT.Complex.Logical.Test
{ 
    [ICO_AOPEnable(true)]
    public interface ITest :  ITransientLifetimeManagerRegister
    {
        int Test();

       // int Test1(ref string iii);
        //object EntityTest();
       // List<object> ListTest();
    }
}

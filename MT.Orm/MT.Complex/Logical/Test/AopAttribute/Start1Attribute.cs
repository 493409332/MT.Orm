using Complex.Logical;
using MT.AOP.Context;


namespace MT.Complex.Logical.Test.AopAttribute
{
    public class Start1Attribute : BaseStartAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
            //---Start1

            //Console.WriteLine("log start!");
            context.IsRun = true;
            return context;
        }
    }
}

using Complex.Logical;
using MT.AOP.Context;


namespace MT.Complex.Logical.Admin.AopAttribute
{
    public class StartAttribute : BaseStartAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
           

            //Console.WriteLine("log start!");
            context.IsRun = true;
            return context;
        }
    }
}

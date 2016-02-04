using Complex.Logical;
using MT.AOP.Context;


namespace MT.Complex.Logical.Admin.AopAttribute
{
    public class EndAttribute : BaseEndAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
          //  Console.WriteLine("log start!");
            return context;
        }
    }
}

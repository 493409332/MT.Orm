using Complex.Logical;
using MT.AOP.Context;


namespace MT.Complex.Logical.Admin.AopAttribute
{
    public class ExAttribute : BaseExAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
            //Console.WriteLine("log exception!");

            //// context.Result
            ////  context.Result=Activator.CreateInstance(context.ResultType);
            //if ( context.ResultType == typeof(decimal) )
            //{
            //    context.Result = 111111.11111M;
            //}
            ////  throw context.Ex.Ex;


            return context;
        }
    }
}

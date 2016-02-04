using MT.AOP.Attribute;
using MT.AOP.Context;
using System;
 

namespace Complex.Logical
{
   public class BaseExAttribute : ExceptionAspectAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
            Console.WriteLine("log exception!");

            throw context.Ex;

        }
    }
}

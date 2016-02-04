using MT.AOP.Attribute;
using MT.AOP.Context;
using System;
 

namespace Complex.Logical
{
  
    public class BaseEndAttribute : PostAspectAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
            Console.WriteLine("log start!");
            return context;
        }
    }
}

using MT.AOP.Attribute;
using MT.AOP.Context;
 
namespace Complex.Logical
{ 
    public class BaseStartAttribute : PreAspectAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        { 
            context.IsRun = true;
            return context;
        }
    }
}


using Complex.Logical;
using MT.AOP.Context;
using MT.Complex.Logical.Enumspace;

namespace MT.Complex.Logical.Admin.AopAttribute
{
    public class UniqueExAttribute : BaseExAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
            var innerex = context.Ex.InnerException.InnerException;
            if (innerex != null)
            {
                if (innerex.Message.Contains("IX_UserName") || innerex.Message.Contains("IX_ButtonTag"))
                {
                    context.Result = CRUDState.UniqueErro;
                }
            }
            return context;
        }
    }
}

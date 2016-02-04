using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_Test
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }


}
namespace MtAop
{
    using MT.Complex.Logical.Test;
    using MT.Complex.Logical.Test.Realization;
    using MT.ICO_AOP.AOP.Attribute;
    using MT.ICO_AOP.AOP.Context;
    using System;
    using System.Reflection;

    public sealed class DynamicRTestType : ITest
    {
        private RTest _realProxy = null;

        public DynamicRTestType()
        {
            this._realProxy = new RTest();
        }

        public  int Test()
        {
            InvokeContext context = new InvokeContext();
            context.SetMethod("Test");
            context.SetClassName("MT.Complex.Logical.Test.Realization.RTest");
            context.ResultType = typeof(int);
            int result = (int) Activator.CreateInstance(Type.GetType("System.Int32"));
            Exception e = null;
            Type[] types = new Type[0];
            MethodInfo method = this._realProxy.GetType().GetMethod("Test", types);

            Attribute[] querAtrr = Attribute.GetCustomAttributes(method, typeof(PreAspectAttribute));

            for ( int i = 0; i < querAtrr.Length; i++ )
            {
                PreAspectAttribute customAttribute = (PreAspectAttribute) querAtrr[i];

                if ( customAttribute != null )
                {
                    context = customAttribute.Action(context);
                }
                if ( !context.IsRun )
                {
                    return (int) Activator.CreateInstance(Type.GetType("System.Int32"));
                }
            }
         
            
             
            try
            {
                result = this._realProxy.Test();
                context.SetResult(result);
                PostAspectAttribute attribute2 = (PostAspectAttribute) Attribute.GetCustomAttribute(method, typeof(PostAspectAttribute));
                if ( attribute2 != null )
                {
                    context = attribute2.Action(context);
                    result = Convert.ToInt32(context.Result);
                }
            }
            catch ( Exception exception1 )
            {
                e = exception1;
                context.SetError(e);
                ExceptionAspectAttribute attribute3 = (ExceptionAspectAttribute) Attribute.GetCustomAttribute(method, typeof(ExceptionAspectAttribute));
                if ( attribute3 == null )
                {
                    throw e;
                }
                return Convert.ToInt32(attribute3.Action(context).Result);
            }
            return result;
        }

        #region ITest 成员


        public object EntityTest()
        {
            throw new NotImplementedException();
        }

        public List<object> ListTest()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using Complex.Logical;
using MT.AOP.Context;


namespace MT.Complex.Logical.Test.AopAttribute
{
    public class StartAttribute : BaseStartAttribute
    {
        public override InvokeContext Action(InvokeContext context)
        {
           
      
           // GCHandle handle = GCHandle.Alloc(context.Parameters[0]);
           // IntPtr ptr = GCHandle.ToIntPtr(handle);
           // char[] aa="asdasdasd".ToCharArray();
          

           //if ( context.Parameters[0] is string )
           //{
           //       string  cccc=  Marshal.PtrToStringAuto(ptr,3);
           //}

          //  Marshal.Copy(aa, 0, ptr, 5);
            //---Start
         
               context.IsRun = true;
              return context;
            //unsafe
            //{
            //    char*[] aaa = (char*[]) context.Parameters[0];

            //    //Console.WriteLine("log start!");
             
            //}
        }
    }
}

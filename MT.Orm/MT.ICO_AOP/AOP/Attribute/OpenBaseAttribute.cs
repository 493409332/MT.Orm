using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.ICO_AOP.AOP.Attribute
{
    /// <summary>
    /// 开启继承方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OpenBaseAttribute : System.Attribute
    {
    }
}

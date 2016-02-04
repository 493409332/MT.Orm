using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.ICO_AOP.ModelAOP.Attribute
{
    /// <summary>
    /// 切片特性
    /// </summary>
    public abstract class AopAttribute : System.Attribute
    {

        public virtual object Action(object context) { return context; }
    }
    /// <summary>
    /// 方法执行前
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class PreAspectAttribute : AopAttribute
    {
    }
    /// <summary>
    /// 方法执行后
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class PostAspectAttribute : AopAttribute
    {
    }
    /// <summary>
    /// 方法执行异常
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ExceptionAspectAttribute : AopAttribute
    {
    }
}

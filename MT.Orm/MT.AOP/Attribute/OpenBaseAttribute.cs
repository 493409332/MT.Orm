using System;

namespace MT.AOP.Attribute
{
    /// <summary>
    /// 开启继承方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OpenBaseAttribute : System.Attribute
    {
    }
}

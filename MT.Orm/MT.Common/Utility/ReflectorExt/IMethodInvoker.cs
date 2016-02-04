namespace MT.Common.Utility.ReflectorExt
{
    /// <summary>
    /// 方法调用器接口
    /// </summary>
    public interface IMethodInvoker
    {
        object Invoke(object instance, params object[] parameters);
    }
}

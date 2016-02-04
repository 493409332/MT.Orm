using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.ICO_AOP.ICO.Attribute
{
    /// <summary>
    ///ICO注册工厂提供允许注册与AOP拦截是否开启的权限 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ICO_AOPEnableAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary> 
        /// <param name="aopEnable">是否允许(默认允许)</param>
        public ICO_AOPEnableAttribute( bool aopEnable = true)
        {
           
            this.AOPEnable = aopEnable;
        }
        /// <summary>
        /// 是否允许
        /// </summary>
        public bool AOPEnable
        {
            get;
            set;
        }

    }
}

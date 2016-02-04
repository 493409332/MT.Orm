using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Orm.DBAttribute
{ 
     /// <summary>
    /// 提供对DBSet的标记功能
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DBSetAttribute : System.Attribute
    {

    }
}

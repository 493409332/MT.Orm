using System;



namespace MT.Orm.DBAttribute
{ 
    /// <summary>
    /// 提供对应映射信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description">映射表名</param>
        public TableAttribute()
        {
            //try
            //{ 
            //TableAttr=JSONhelper.ConvertToObject<TableAttr>(Json); 
            //}
            //catch ( Exception )
            //{ 
            //    throw new Exception("json格式错误");
            //}
        }
        /// <summary>
        /// 映射表名
        /// </summary>
        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// 表描述
        /// </summary>
        public string TableComment
        {
            get;
            set;
        }

    }

}

using System;

namespace MT.Orm.DBAttribute
{ 
    /// <summary>
    /// 提供对应映射信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description">映射列名</param>
        public ColumnAttribute()
        {
            //  this.ColumnName = Json;
        }

        /// <summary>
        /// 映射列名
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 长度
        /// </summary>
        public string MaxLength
        {
            get;
            set;
        }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public Boolean Unique
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可为空
        /// </summary>
        public Boolean NotNull
        {
            get;
            set;
        }
        /// <summary>
        /// 是否标识
        /// </summary>
        public Boolean Identity
        {
            get;
            set;
        }
        /// <summary>
        /// 主键
        /// </summary>
        public Boolean Primary
        {
            get;
            set;
        }

    }


}

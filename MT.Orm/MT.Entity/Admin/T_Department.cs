using MT.Orm.DBAttribute;
using System.Collections.Generic;

namespace Complex.Entity.Admin
{
    [Table(TableName = "T_Department", TableComment = "部门管理")]
    public class T_Department : EntityBase
    {
        [Column(ColumnName = "DepartmentName", Description = "部门名称", MaxLength = "50")]
        public string DepartmentName
        {
            get;
            set;
        }
        [Column(ColumnName = "ParentId", Description = "上级ID", NotNull = true)]
        public int ParentId
        {
            get;
            set;
        }
        [Column(ColumnName = "Sortnum", Description = "排序", NotNull = true)]
        public int Sortnum
        {
            get;
            set;
        }
        [Column(ColumnName = "Remark", Description = "备注", MaxLength = "200")]
        public string Remark
        {
            get;
            set;
        }
        public IEnumerable<T_Department> children
        {
            get;
            set;
        }
        public string text
        {
            get { return DepartmentName; }

        }
        // [NotMapped]
        ///// <summary>
        ///// tree 节点状态
        ///// </summary>
        //public string state
        //{
        //    get
        //    {
        //        if ( ParentId == 0 )
        //            return "open";
        //        return children.Any() ? "closed" : "open";
        //    }
        //}
    }
}

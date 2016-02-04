using MT.Orm.DBAttribute;

namespace Complex.Entity.Admin
{
    [Table(TableName = "T_Role", TableComment = "角色管理")]
    public class T_Role : EntityBase
    {
        [Column(ColumnName = "RoleName", Description = "角色名称", MaxLength = "50")]
        public string RoleName
        {
            get;
            set;
        }
        [Column(ColumnName = "Sortnum", Description = "排序")]
        public int Sortnum
        {
            get;
            set;
        }
        [Column(ColumnName = "Remark", Description = "描述", MaxLength = "200")]
        public string Remark
        {
            get;
            set;
        }
        [Column(ColumnName = "IsDefault", Description = "是否为默认角色")]
        public int IsDefault
        {
            get;
            set;
        }
        [Column(ColumnName = "DepartmentId", Description = "部门ID")]
        public int DepartmentId
        {
            get;
            set;
        }
        //public IEnumerable<Navigation> Navigations { get; set; }
        //public IEnumerable<User> Users { get; set; }
        ///// <summary>
        ///// 角色可以访问的部门列表
        ///// </summary>

        //public string Departments
        //{
        //    get { return RoleDal.Instance.GetDepIDs(KeyId); }
        //}
    }
}

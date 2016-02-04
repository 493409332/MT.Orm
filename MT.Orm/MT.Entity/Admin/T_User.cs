using MT.Orm.DBAttribute;

namespace Complex.Entity.Admin
{
    [Table(TableName = "T_User", TableComment = "系统用户")]
    public class T_User : EntityBase
    {

        [Column(ColumnName = "UserName", Description = "用户名", MaxLength = "50", NotNull = true, Unique = true)]
        public string UserName
        {
            get;
            set;
        }
        [Column(ColumnName = "Password", Description = "密码", MaxLength = "64", NotNull = false)]
        public string Password
        {
            get;
            set;
        }
        [Column(ColumnName = "TrueName", Description = "真实姓名")]
        public string TrueName
        {
            get;
            set;
        }

        [Column(ColumnName = "PassSalt", Description = "密码佐料")]
        public string PassSalt
        {
            get;
            set;
        }
        [Column(ColumnName = "Email", Description = "Email")]
        public string Email
        {
            get;
            set;
        }
        [Column(ColumnName = "IsAdmin", Description = "是否超管")]
        public bool IsAdmin
        {
            get;
            set;
        }
        [Column(ColumnName = "IsDisabled", Description = "是否禁用")]
        public bool IsDisabled
        {
            get;
            set;
        }
        [Column(ColumnName = "DepartmentId", Description = "部门Id")]
        public int DepartmentId
        {
            get;
            set;
        }
        [Column(ColumnName = "Mobile", Description = "手机")]
        public string Mobile
        {
            get;
            set;
        }
        [Column(ColumnName = "QQ", Description = "QQ")]
        public string QQ
        {
            get;
            set;
        }
        [Column(ColumnName = "Remark", Description = "备注")]
        public string Remark
        {
            get;
            set;
        }
        [Column(ColumnName = "ConfigJson", Description = "个性化设置")]
        public string ConfigJson
        {
            get;
            set;
        }
        //聚合索引   
        //[Index("IX_UserName", IsUnique = true, Order = 1)] 
        //public string UserName { get; set; }  
        //[Index("IX_UserName", IsUnique = true, Order = 2)] 
        //public string TrueName { get; set; }
    }

    public class UserConfig
    {
        public theme theme { get; set; }
        public string showType { get; set; }
        public string gridRows { get; set; }
    }
    public class theme
    {

        public string title { get; set; }
        public string name { get; set; }
    }

}

using MT.Orm.DBAttribute;

namespace Complex.Entity.Admin
{
    [Table(TableName = "T_UserRoles", TableComment = "用户角色关联表")]
    public class T_UserRoles:EntityBase
    {
        [Column(ColumnName = "UserID", Description = "用户ID")]
        public int UserID 
        { 
            get; 
            set; 
        }
        [Column(ColumnName = "RoleID", Description = "角色ID")]
        public int RoleID 
        { 
            get; 
            set; 
        }
    }
}

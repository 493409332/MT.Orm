using Complex.Entity.Admin;
using MT.Entity;
using MT.Orm;

namespace MT.Complex.Repository
{
 
    public class EntitytoData : DBContext
    {
        public EntitytoData() : base("SQLServerContext")
        {

        }
        public EntitytoData(string p)
            : base(p)
        {

        }
        public DBSet<T_User> T_User { get; set; }

        public DBSet<T_Button> T_Button { get; set; }

        public DBSet<T_Navigation> T_Navigation { get; set; }

        public DBSet<T_NavButtons> T_NavButtons { get; set; }

        public DBSet<T_Department> T_Department { get; set; }

        public DBSet<T_Role> T_Role { get; set; }

        public DBSet<T_RoleNavBtns> T_RoleNavBtns { get; set; }

        public DBSet<T_UserRoles> T_UserRoles { get; set; }


      
    }

}

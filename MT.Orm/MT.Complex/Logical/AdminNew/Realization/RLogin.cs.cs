using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.Orm;
using System.Linq;

namespace Complex.Logical.Admin.Realization
{ 
    [ICOConfig("RLogin")]
    public class RLogin : RBase<T_User>, ILogin
    {
        public RLogin() 
        {
        }
        #region ILogin 成员

        public bool SearchForName(string username)
        {
            return GetAll().Where(p => p.UserName == username && p.IsDelete == false).Count() > 0;

        }
        public T_User SearchForNameorPwd(string username, string userpwd)
        {
            return GetAll().Where(p => p.UserName == username && p.Password == userpwd && p.IsDelete == false).ToList().FirstOrDefault();
        }


        #endregion

      
    }
}

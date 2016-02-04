using Complex.Entity.Admin;
using MT.Complex.Logical.Admin.AopAttribute;
using MT.ICO.Attribute;
using System.Collections.Generic;
using MT.Orm;
using System.Linq;
namespace Complex.Logical.Admin.Realization
{


    [ICOConfig("RUser")]
    public class RUser : RBase<T_User>, IUser
    {
        public RUser() 
        {
        }
        #region IUser 成员
        public List<T_UserTem> GetAllUser()
        {
            List<T_UserTem> quer = new List<T_UserTem>();
            //quer = EF.Database.SqlQuery<T_UserTem>("select a.*,b.DepartmentName from T_User a left join T_Department b on a.DepartmentId=b.ID").ToList();
            return EF.Database.ExecutiveSQL<T_UserTem>("select a.* from T_User a where a.IsDelete='false'");

          //  return null;
        }
        public T_User GetUserByKey(int UserID)
        {
            var quer = GetByKey(UserID);
            return quer;
        }

        [T_UserInfoCache]
        public int SetUseConfigrByKey(int UserID, string json)
        {
            var quer = GetByKey(UserID);
            quer.ConfigJson = json;
            return Update(quer);
        }
        public string GetConfig(int UserID)
        {
            var quer = GetAll().Where(p => p.ID == UserID).ToList().FirstOrDefault();
            if (quer != null)
            {
                return quer.ConfigJson != null ? quer.ConfigJson : "{ \"theme\": { \"title\": \"Bootstrap\", \"name\": \"bootstrap\" }, \"showType\": \"tree\", \"gridRows\": \"20\" }";
            }
            return "{ \"theme\": { \"title\": \"Bootstrap\", \"name\": \"bootstrap\" }, \"showType\": \"tree\", \"gridRows\": \"20\" }";
        }

        #endregion
        //public List<T_Button> GetPage(string predicate, int page, int page_size, string order, string asc)
        //{
        //  //  return GetAllNoCache().OrderBy(p=>p.ID).Skip(( page - 1 ) * page_size).Take(page_size).ToList();

        //    return SearchSqLFor_Page<T_Button>("ID>1",2,5,"ID","asc"); 
        //}








        public T_User GetByKey(int UserID)
        {
            return GetAll().Where(p => p.ID == UserID).ToList().FirstOrDefault();
        }


    }
}

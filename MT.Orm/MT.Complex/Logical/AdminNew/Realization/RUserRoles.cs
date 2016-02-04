using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complex.Entity.Admin;
//using Complex.ICO_AOP;
//using Complex.ICO_AOP.Attribute;
//using Complex.Logical.Admin.AopAttribute;
//using Complex.Repository;
using MT.ICO.Attribute;
using MT.Orm;
using MT.Complex.Logical.Admin.AopAttribute;
namespace Complex.Logical.Admin.Realization
{
    [ICOConfig("RUserRoles")]
    public class RUserRoles : RBase<T_UserRoles>, IUserRoles
    {
        public RUserRoles() 
        {

        }

        //public List<T_Button> GetPage(string predicate, int page, int page_size, string order, string asc)
        //{
        //  //  return GetAllNoCache().OrderBy(p=>p.ID).Skip(( page - 1 ) * page_size).Take(page_size).ToList();

        //    return SearchSqLFor_Page<T_Button>("ID>1",2,5,"ID","asc"); 
        //}

        public List<T_UserRoles> GetRoleByUserID(int UserID)
        {
            return GetAll().Where(p => p.UserID == UserID).ToList();
        }


        [T_UserInfoCache]
        public int AddUserTo(int UserID, int[] roleids)
        {
            
            var Error = 0;

            List<TransactionHelp> tranlist = new List<TransactionHelp>();
            if (GetAll().Where(p => p.UserID == UserID).Count() > 0)
            {
                var rows = EF.Database.TransactionExecuteSqlCommand(ref tranlist, "delete from T_UserRoles where UserID=" + UserID);
                if (rows == 0)
                {
                    Error++;
                }
            }
       
            foreach (var item in roleids)
            {
                T_UserRoles model = new T_UserRoles();
                model.UserID = UserID;
                model.RoleID = item; 
                if (Insert(model, ref tranlist)<=0)
                {
                    Error++;
                }  
            }
            EF.Transaction(tranlist, Error == 0);

            return Error==0 ? 1 : 0;
        }
    }
}

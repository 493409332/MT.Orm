using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complex.Entity.Admin;
using MT.Orm;
using MT.ICO.Attribute;
using MT.Complex.Logical.Admin.AopAttribute;
using Newtonsoft.Json.Linq;
using MT.Common.Utility.ConvertExt;
namespace Complex.Logical.Admin.Realization
{
    [ICOConfig("RRoleNavBtns")]
    public class RRoleNavBtns : RBase<T_RoleNavBtns>, IRoleNavBtns
    {
        public RRoleNavBtns() 
        {

        }
       #region IRoleNavButtons 成员
        public List<T_RoleNavBtns> GetRoleNavBtns()
        {
            return GetAll().Where(p=>p.IsDelete == false).ToList();
        }

        public List<T_RoleNavBtns> GetRoleNavBtnsByRole(int roleID)
        {
            return GetAll().Where(p => p.RoleID == roleID && p.IsDelete == false).ToList();
        }


        [T_UserInfoCache]
        public bool setRoleButtons(string Data)
        {
            JObject jobj = JObject.Parse(Data);
            int roleID = jobj["roleId"].ReferenceFromType<JToken, int>();
            var menus = jobj["menus"];

            var buttons = GetAll<T_Button>().OrderBy(p => p.Sortnum, false).ToList();
            

            var navs = menus.Select(menu => new
            {
                navid = menu["navid"].ReferenceFromType<JToken, int>(),
                btns = buttons.Where(n =>
                        menu["buttons"].Select(m => (string) m).Contains<string>(n.ButtonTag)
                        ).Select(k => k)
            });

          
            int Erro = 0;


            List<TransactionHelp> tranlist = new List<TransactionHelp>();
            foreach ( var nav in navs )
            {
                if (GetAll().Where(p => p.NavID == nav.navid && p.RoleID == roleID).Count() > 0)
                {
                    if (EF.Database.TransactionExecuteSqlCommand(ref tranlist, "delete from T_RoleNavBtns where NavId=" + nav.navid + " and RoleID=" + roleID) == 0)
                    {
                        Erro++;
                    }
                }

                foreach ( var btn in nav.btns )
                {
                    if (Add(new T_RoleNavBtns() { RoleID = roleID, NavID = nav.navid, BtnID = btn.ID }, ref tranlist) < 1)
                    {
                        Erro++;
                    }
                }
            }

            EF.Transaction(tranlist, Erro == 0);
            return Erro == 0; 
          
        }
         #endregion


    }
}

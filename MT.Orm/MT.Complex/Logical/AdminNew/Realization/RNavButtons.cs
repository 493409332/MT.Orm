using Complex.Entity.Admin;
using MT.Complex.Logical.Admin.AopAttribute;
using MT.ICO.Attribute;
using MT.Orm;
using System.Collections.Generic;

namespace Complex.Logical.Admin.Realization
{

    [ICOConfig("RNavButtons")]
    public class RNavButtons : RBase<T_NavButtons>, INavButtons
    {
        public RNavButtons() 
        {
        }

        #region INavButtons 成员

        public List<T_NavButtons> GetButByNavID(int NavID)
        {
            return GetAll().Where(p => p.NavId == NavID && p.IsDelete == false).ToList();
        }

        [T_UserInfoCache]
        public bool setButtons(int NavID, int[] btns)
        {
            int Erro = 0;
            List<TransactionHelp> tranlist = new List<TransactionHelp>();
            if (btns.Length > 0 && NavID > 0)
            {
                if (GetAll().Where(p => p.NavId == NavID).Count() > 0)
                {
                    if (EF.Database.TransactionExecuteSqlCommand(ref tranlist, "delete from T_NavButtons where NavId=" + NavID) == 0)
                    {
                        Erro++;
                    }
                } 
                foreach (var item in btns)
                {
                    if (Add(new T_NavButtons() { NavId = NavID, ButtonId = item }, ref tranlist) < 1)
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

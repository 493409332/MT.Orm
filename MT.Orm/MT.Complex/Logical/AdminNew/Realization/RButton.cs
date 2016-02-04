using System.Collections.Generic;
using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.Orm;

namespace Complex.Logical.Admin.Realization
{
    [ICOConfig("RButton")]
    public class RButton : RBase<T_Button>, IButton
    {
        public RButton() 
        { 
        }

        //public List<T_Button> GetPage(string predicate, int page, int page_size, string order, string asc)
        //{
        //  //  return GetAllNoCache().OrderBy(p=>p.ID).Skip(( page - 1 ) * page_size).Take(page_size).ToList();
           
        //    return SearchSqLFor_Page<T_Button>("ID>1",2,5,"ID","asc"); 
        //}


        public List<T_Button> GetButtons()
        {
            return GetAll().OrderBy(p => p.ID).ToList();
        }
        
    }
}

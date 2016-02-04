using Complex.Entity.Admin;
using MT.Complex.Logical.Test.AopAttribute;
using MT.Complex.Repository;
using MT.Entity;
using MT.ICO.Attribute;
using MT.Orm;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace MT.Complex.Logical.Test.Realization
{
    [ICOConfig("RTest")]
    public class RTest : RepositoryBase<T_User>, ITest
    {

        #region ITest 成员
        [Start1, Start, End]
        public int Test()
        {
            #region
               // EF.EntityTest.GetAllQUeryable();//OrderBy(p => new { aaa = p.Column1 }).Skip(1);
               //EF.EntityTest.GetAllQUeryable().Where(p => p.aaaaaa1 == Hehe.hehe1 && p.Column1 == 1 && p.Column3333 >= 5 || p.Column4444 >= 5 && p.Column123123 == "aaa").GroupBy(p => new {_Column1=DBFuc.DBCount( p.Column1) }).ToList();
               //EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").OrderBy(p => new { p.Column1 }).Skip(2).Take(1).Select(p => new { AAA=p.aaaaaa}).ToList();//.Select(p => new { aaa = p.aaaaaa1, bbb = p.Column1 });//.ToList();//Select(p => new {aaa=p.aaaaaa1,bbb=p.Column1});
               // EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").OrderBy(p => new { p.Column1 }).Skip(2).Select(p => new { AAA=p.aaaaaa}).ToList();//.Select(p => new { aaa = p.aaaaaa1, bbb = p.Column1 });//.ToList();//Select(p => new {aaa=p.aaaaaa1,bbb=p.Column1});
               // EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").OrderBy(p => new { p.Column1 }).Take(2).Select(p => new { AAA = p.aaaaaa }).ToList();
               //EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").Skip(2).Take(1).Select(p => new { AAA = p.aaaaaa }).ToList();
               //EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").Take(1).Select(p => new { AAA = p.aaaaaa }).ToList();
               //EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").Skip(1).Select(p => new { AAA = p.aaaaaa }).ToList();
               // EF.EntityTest.GetAllQUeryable().Select(p => new { AAA = p.aaaaaa }).ToList();//.Select(p => new { aaa = p.aaaaaa1, bbb = p.Column1 });//.ToList();//Select(p => new {aaa=p.aaaaaa1,bbb=p.Column1});
               // EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").Skip(2).ToList();
               // EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").Select(p => new { bbb = p.Column1 }).ToList();
               // EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").OrderBy(p => new { p.Column1 }).Select(p => new { AAA = p.aaaaaa }).ToList();
               //EF.EntityTest.GetAllQUeryable().Where(p => p.Column1 == 1 && p.Column2 == "12").Select(p => new { AAA = p.aaaaaa }).ToList();
               //EF.EntityTest.GetAllQUeryable().Select(p => new { AAA = p.aaaaaa }).ToList();
               // EF.EntityTest.GetAllQUeryable().ToList();
                #endregion

            #region
            List<int> aaa = new List<int>() { 1, 3, 4, 5, 6, 8 }; 
             IList<int> aaa2 = new  List<int>() { 1, 3, 4, 5, 6, 8 }; 
            int[] aaa1 = new int[]{1,3,4,5,6,8};
            #endregion

            //GroupBy(p => new { p.ID, count = p.ButtonId.DBSum() })
         List<Guid> quer=  GetAll<T_NavButtons>().Select(p => p.GuID ).ToList()  ;
           // GetAll<T_NavButtons>().Select(p => new { aaa=p.ID}).ToList();
         int aaa12= quer.Count();
            return 0;
            

        }




        #endregion

        #region ITest 成员

        [Start, Start1, Ex, End]
        public int Test1(ref string iii)
        {
            GCHandle handle = GCHandle.Alloc(iii);
            IntPtr ptr = GCHandle.ToIntPtr(handle);
            char[] aa = "asdasdasd".ToCharArray();

            iii = "1000adssd";
            return 1;
        }

        #endregion

        #region ITest 成员


        public List<object> ListTest()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

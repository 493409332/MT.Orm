using MT.AOP.Attribute;
using MT.Common.Utility.LinqExt;
using MT.Complex.Logical.Admin.AopAttribute;
using MT.Complex.Repository;
using MT.Orm;
using System;
using System.Collections.Generic;

namespace Complex.Logical.Admin.Realization
{
    public abstract class RBase<T> : RepositoryBase<T>, IBase<T> where T : Complex.Entity.EntityBase,new()
    {
        public RBase(): base("SQLServerContext")
        {

        }
        public RBase(string strsql)
            : base(strsql)
        {

        }
        #region IBase<T> 成员
        [OpenBase]
        [T_UserInfoCache,UniqueEx]
        public virtual int Add(T model)
        {
            return Insert(model);
        }

        [OpenBase]
        [T_UserInfoCache, UniqueEx]
        public virtual int Add(T model, ref  List<TransactionHelp> tranlist)
        {
            return Insert(model, ref tranlist);
        }
        [OpenBase]
        [T_UserInfoCache]
        public virtual int Remove(int ID)
        { 
            return FalseDeleteByKey(ID);
        }
        [OpenBase]
        [T_UserInfoCache]
        public virtual int Remove(int ID, ref  List<TransactionHelp> tranlist)
        {
            return FalseDeleteByKey(ID, "IsDelete", ref tranlist);
        }
        [OpenBase]
        [T_UserInfoCache]
        public virtual int TrueRemove(int ID)
        {
            return DeleteByKey(ID);
        }
        [OpenBase]
        [T_UserInfoCache]
        public virtual int TrueRemove(int ID, ref  List<TransactionHelp> tranlist)
        {
            return DeleteByKey(ID, ref tranlist);
        }
        [OpenBase]
        [T_UserInfoCache, UniqueEx]
        public virtual int Edit(T model)
        {
            return Update(model);
        }
        [OpenBase]
        [T_UserInfoCache, UniqueEx]
        public virtual int Edit(T model, ref  List<TransactionHelp> tranlist)
        {
            return Update(model, ref tranlist);
        }
        [OpenBase]
        public virtual List<T> GetAllList()
        {
            //return GetAllNoCache().Where(p => p.IsDelete == false).ToList();

            return GetAll().Where(p => p.IsDelete == false).ToList();
        }
        [OpenBase]
        public virtual IEnumerable<T> GetAllEnumerable()
        {
            return GetAll().Where(p => p.IsDelete == false).ToList();
        }
        [OpenBase]
        public virtual List<T> GetPage(string predicate, int page, int page_size, string order, string asc, out int total)
        {

            return SearchSqLFor_Page(FilterTranslator.ToSql(predicate), page, page_size, order, asc, out total);

            // throw new NotImplementedException();
        }

        [OpenBase]
        public virtual List<T> GetPageList(T model, int page, int rows, string sort, string order, out int total, string[] where)
        {
            throw new NotImplementedException();
        }
        [OpenBase]
        public virtual IEnumerable<T> GetPageEnumerable(T model, int page, int rows, string sort, string order, out int total, string[] where)
        {
            throw new NotImplementedException();
        }
        #endregion
    













    }
}

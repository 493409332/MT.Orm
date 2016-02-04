using MT.Orm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MT.Complex.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
         where TEntity : class ,new()
    {
         public EntitytoData EF ;
        

         public RepositoryBase()
         {
             EF = new EntitytoData();
         }
         public RepositoryBase(string connstr)
         { 
             EF = new EntitytoData(connstr);
         }

        #region IRepository<TEntity> 成员

        public Orm.DBSet<TEntity> Entities
        {
            get { return EF.Set<TEntity>(); }
        }

        public int Insert(TEntity entity)
        {
          return  Entities.Insert(entity);
        }
        public int Insert(TEntity model, ref  List<TransactionHelp> tranlist)
        {
            return Entities.Insert(model, ref tranlist);
        }
        public int Insert(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity)
        {
            return Entities.Update(entity);
        }
        public int Update(TEntity entity, ref  List<TransactionHelp> tranlist)
        {
            return Entities.Update(entity, ref tranlist);
        }
        public int DeleteByKey(object ID )
        {
            return Entities.DeleteByKey(ID);
        }

        public int DeleteByKey(object ID, ref  List<TransactionHelp> tranlist)
        {
            return Entities.DeleteByKey(ID, ref tranlist);
        }

        public int FalseDeleteByKey(object ID, string columstr = "IsDelete")
        {
            return Entities.FalseDeleteByKey(ID, columstr);
        }

        public int FalseDeleteByKey(object ID, string columstr, ref  List<TransactionHelp> tranlist)
        {
            return Entities.FalseDeleteByKey(ID, columstr, ref tranlist);
        }
        public TEntity GetByKey(TEntity entity)
        {
            return Entities.FindByID(entity);
        }

        public IQUeryable<TEntity> GetAll()
        {

            return Entities.GetAllQUeryable();
        }
        public IQUeryable<TEntity1> GetAll<TEntity1>() where TEntity1 : class ,new()
        {
            return EF.Set<TEntity1>().GetAllQUeryable();
        }

        public List<TEntity> GetByProperty(TEntity entity) {
            return Entities.FindByProperty(entity);
        }

        public List<TEntity> ExecutiveSQL(string sql, SqlParameter[] paras) {

            return Entities.ExecutiveSQL(sql, paras);
        }

 
       
        public List<TEntity> SearchSqLFor_Page(string predicate, int page, int page_size, string order, string asc,out int total)
        {
            return Entities.SearchSqLFor_Page(predicate, page, page_size, order, asc,out total);
        }

        #endregion

        #region IRepository<TEntity> 成员



        public TEntity GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        #endregion





        Orm.DBSet<TEntity> IRepository<TEntity>.Entities
        {
            get { throw new NotImplementedException(); }
        }

        int IRepository<TEntity>.Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        int IRepository<TEntity>.Insert(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        int IRepository<TEntity>.Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        int IRepository<TEntity>.DeleteByKey(object id)
        {
            throw new NotImplementedException();
        }

        TEntity IRepository<TEntity>.GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        List<TEntity> IRepository<TEntity>.SearchSqLFor_Page(string predicate, int page, int page_size, string order, string asc, out int total)
        {
            throw new NotImplementedException();
        }

        #region IRepository<TEntity> 成员


        public List<T> SearchSqLFor_Page<T>(string predicate, int page, int page_size, string order, string asc, out int total) where T : class, new()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

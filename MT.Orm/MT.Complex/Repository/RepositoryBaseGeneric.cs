using MT.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MT.Complex.Repository
{
    

    public abstract class RepositoryBaseGeneric
    {
       
        public EntitytoData EF;

        public RepositoryBaseGeneric()
        {
            EF = new EntitytoData("SQLServerContext");
        }
        public RepositoryBaseGeneric(string connstr)
        {
            EF = new EntitytoData(connstr);
        }
        public void ChangeDatabase(string connstr)
        {
            EF = new EntitytoData(connstr);
        }


        public int Insert<TEntity1>(TEntity1 entity) where TEntity1 : class,new()
        {
            return EF.Set<TEntity1>().Insert(entity); 
        }


        public int Insert<TEntity1>(IEnumerable<TEntity1> entities) where TEntity1 : class,new()
        {
            return 0; 
        }




        public IQUeryable<TEntity1> SearchFor<TEntity1>(Expression<Func<TEntity1, bool>> predicate) where TEntity1 : class,new()
        {
            return EF.Set<TEntity1>().GetAllQUeryable().Where(predicate).ToIQUeryable();
        }





        public IQUeryable<TEntity1> GetAllNoCache<TEntity1>() where TEntity1 : class,new()
        {
            return EF.Set<TEntity1>().GetAllQUeryable();
        }



        public IQUeryable<TEntity1> GetAll<TEntity1>() where TEntity1 : class,new()
        {
            return EF.Set<TEntity1>().GetAllQUeryable();
        }

 



        //key删除
        public int DeleteByKey<TEntity1>(object id) where TEntity1 : class,new()
        {
            ///删除操作实现 

            return EF.Set<TEntity1>().DeleteByKey(id);
        }


        //public TEntity1 GetByKey<TEntity1>(object key) where TEntity1 : class,new()
        //{
        //    return EF.Set<TEntity1>().FindByID(key);
        //}



        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update<TEntity1>(TEntity1 entities) where TEntity1 : class,new()
        {
            return EF.Set<TEntity1>().Update(entities);

             
        }

        ///// <summary>
        ///// 分页  
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <param name="page"></param>
        ///// <param name="page_size"></param>
        ///// <returns></returns>
        //public List<TEntity1> SearchSqLFor_Page<TEntity1>(string predicate, int page, int page_size, string order, string asc, out int total) where TEntity1 : class,new()
        //{
        //    string Sql = string.Empty;
        //    string SqlCount = string.Empty;
        //    if (EF.Database.Connection.ToString().StartsWith("System.Data.SqlClient"))
        //    {
        //        Sql = string.Format(" SELECT TOP {0}  * FROM ( SELECT *, row_number() OVER (ORDER BY {1} {2}) AS [row_number]   FROM [dbo].[{5}] AS [Extent1]  where IsDelete='False' and {3} )  AS [Extent1]    WHERE [Extent1].[row_number] > {4}   ORDER BY [Extent1].{1} {2}", page_size, order.Replace("'", "''"), asc.Replace("'", "''"), predicate, (page - 1) * page_size, typeof(TEntity1).Name);
        //        //SqlCount = string.Format(" select COUNT(1) from {0} where {1}", typeof(T).Name, predicate);
        //        SqlCount = string.Format(" select COUNT(1) from {0} where IsDelete='False' and {1}", typeof(TEntity1).Name, predicate);
        //    }
        //    else if (EF.Database.Connection.ToString().StartsWith("Oracle.ManagedDataAccess.Client"))
        //    {
        //        Sql = "";
        //        throw new Exception("目前未实现");
        //    }
        //    List<TEntity1> list = new List<TEntity1>();
        //    try
        //    {
        //        total = EF.Database.SqlQuery<int>(SqlCount).FirstOrDefault();
        //        list = EF.Database.SqlQuery<TEntity1>(Sql).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        total = 0;
        //    }
        //    return list;

        //}

        //public int Update<TEntity1>(object Id, TEntity1 entities) where TEntity1 : class,new()
        //{

        //    if (EF == null) throw new ArgumentNullException("dbContext");
        //    if (entities == null) throw new ArgumentNullException("entities");


        //    DbSet<TEntity1> dbSet = EF.Set<TEntity1>();
        //    try
        //    {
        //        System.Data.Entity.Infrastructure.DbEntityEntry<TEntity1> entry = EF.Entry(entities);
        //        if (entry.State == EntityState.Detached)
        //        {
        //            dbSet.Attach(entities);
        //            entry.State = EntityState.Modified;
        //        }
        //    }
        //    catch (InvalidOperationException e)
        //    {
        //        TEntity1 oldEntity = dbSet.Find(Id);
        //        EF.Entry(oldEntity).CurrentValues.SetValues(entities);

        //    }
        //    return SaveChanges();

        //}

        ////id
        //public TEntity GetById(int key)
        //{ 
        //    return Entities.Single(e => e.Id.Equals(key)); 
        //}
        ////id
        //public int DeleteById(int id)
        //{
        //    ///删除操作实现 
        //    Entities.Remove(GetById(id));
        //    return EF.SaveChanges();
        //}

        //public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQUeryable<TSource> source, Expression<Func<TSource, TKey>> keySelector);
        //public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQUeryable<TSource> source, Expression<Func<TSource, TKey>> keySelector);
    }
}

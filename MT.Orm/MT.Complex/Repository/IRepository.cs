using MT.Orm;
using System.Collections.Generic;

namespace MT.Complex.Repository
{
    internal interface IRepository<TEntity> where TEntity : class,new()
    {
         
         DBSet<TEntity> Entities { get; }
         //增加单个实体
         int Insert(TEntity entity);
        //增加多个实体
         int Insert(List<TEntity> entities);
 

         int Update(TEntity entity);
         //key删除
         int DeleteByKey(object id);

         //根据主键获取实体
         TEntity GetByKey(object key);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <param name="order"></param>
        /// <param name="asc"></param>
        /// <param name="total"></param>
        /// <param name="type"></param>
        /// <returns></returns>
         List<TEntity> SearchSqLFor_Page(string predicate, int page, int page_size, string order, string asc, out int total);


         ////id
         //TEntity GetById(int key);
         ////id
         //int DeleteById(int id);

        // IQueryable<TEntity> SearchFor_Page(Expression<Func<TEntity, bool>> predicate, int page, int page_size);
       // /// <summary>
       // /// 分页
       // /// </summary>
       // /// <typeparam name="TKey"></typeparam>
       // /// <param name="predicate"></param>
       // /// <param name="page"></param>
       // /// <param name="page_size"></param>
       // /// <param name="order"></param>
       // /// <param name="desc"></param>
       // /// <returns></returns>
       // List<TEntity> SearchFor_Page<TKey>(Expression<Func<TEntity, bool>> predicate, int page, int page_size, Expression<Func<TEntity, TKey>> order, bool desc);
       ///// <summary>
       ///// 分页
       ///// </summary>
       ///// <typeparam name="TKey"></typeparam>
       ///// <param name="page"></param>
       ///// <param name="page_size"></param>
       ///// <param name="order"></param>
       ///// <param name="asc"></param>
       ///// <returns></returns>
       // List<TEntity> SearchFor_Page<TKey>(int page, int page_size, Expression<Func<TEntity, TKey>> order, bool asc);
       // /// <summary>
       // /// where查询
       // /// </summary>
       // /// <param name="predicate"></param>
       // /// <returns></returns>
       // IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
       // /// <summary>
       // /// 查询全部
       // /// </summary>
       // /// <returns></returns>
       // IQueryable<TEntity> GetAll();
    }
}

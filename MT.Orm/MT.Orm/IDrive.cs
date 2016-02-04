using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MT.Orm
{
    public interface IDrive
    {
        string ConnectionString { get; set; } 
        bool IsConnectionSuccess();

      

        /// <summary>
        /// Insert  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>int 大于0写入成功  小于0写入失败</returns>
        int Insert<T>(T model) where T : class;


        int Insert<T>(T model, ref  List<TransactionHelp> tranlist) where T : class;
        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>int 受影响的行数 </returns>
        int DeleteByModel<T>(T model) where T : class;


        int DeleteByModel<T>(T model, ref  List<TransactionHelp> tranlist) where T : class;
        /// <summary>
        /// DeleteByKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        int DeleteByKey<T>(object ID) where T : class;

        int DeleteByKey<T>(object ID, ref  List<TransactionHelp> tranlist) where T : class;
        /// <summary>
        /// FalseDeleteByKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <param name="columstr"></param>
        /// <returns></returns>
        int FalseDeleteByKey<T>(object ID, string columstr) where T : class;

        int FalseDeleteByKey<T>(object ID, string columstr, ref  List<TransactionHelp> tranlist) where T : class;
        /// <summary>
        /// Update  修改  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>int 受影响的行数</returns>
        int Update<T>(T model) where T : class;

        int Update<T>(T model, ref  List<TransactionHelp> tranlist) where T : class;
        /// <summary>
        /// FindByID  更具ID查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>List<T></returns>
        T FindByID<T>(T model) where T : class,new();
        /// <summary>
        /// FindByProperty  更具属性查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>List<T> </returns>
        List<T> FindByProperty<T>(T model) where T : class,new();
        /// <summary>
        ///FindAll  查询所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>List<T> </returns>
        List<T> FindAll<T>(T model) where T : class,new();
        /// <summary>
        /// FindLikeProperty  更具属性模糊查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        List<T> FindLikeProperty<T>(T model) where T : class,new();
     /// <summary>
        /// ExecutiveSQL 执行查询SQL语句
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="cmdText"></param>
     /// <param name="paras"></param>
     /// <returns></returns>
        List<T> ExecutiveSQL<T>(string cmdText, SqlParameter[] paras) where T : class,new();


        DataTable ExecutiveSQL(string cmdText,string ConnectionString);

       // List<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class,new();

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <param name="order"></param>
        /// <param name="asc"></param>
        /// <param name="total"></param>
        /// <param name="T"></param>
        /// <returns></returns>
         List<T> SearchSqLFor_Page<T>(string predicate, int page, int page_size, string order, string asc,out int  total) where T : class, new();
         
        void GenerateBuildTable(List<Type> modeltype);

        

        IQUeryable<T> GetAllQUeryable<T>() where T : class, new();

        
    }
}

using MT.Orm.DBAttribute;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MT.Orm
{
    [DBSet]
    public class DBSet<T> where T:class,new()
    {
         private IDrive idrive { get; set; }
      //  public IDrive idrive { get; set; }
        public DBSet()
        {  
        
        }
        public  int Insert(T model){ 
            return idrive.Insert<T>(model);
        }

        public int Insert(T model, ref  List<TransactionHelp> tranlist)
        {
            return idrive.Insert<T>(model, ref tranlist);
        }


        public int Delete(T model) {
            return idrive.DeleteByModel<T>(model);
            
        }
        public int Delete(T model, ref  List<TransactionHelp> tranlist)
        {
            return idrive.DeleteByModel<T>(model, ref tranlist); 
        } 
        public int DeleteByKey(object ID) {

            return idrive.DeleteByKey<T>(ID); 
        }
        public int DeleteByKey(object ID, ref  List<TransactionHelp> tranlist)
        { 
            return idrive.DeleteByKey<T>(ID, ref tranlist);  
        }
        public int FalseDeleteByKey(object ID, string columstr)
        { 
            return idrive.FalseDeleteByKey<T>(ID, columstr); 
        }
        public int FalseDeleteByKey(object ID, string columstr, ref  List<TransactionHelp> tranlist)
        {
            return idrive.FalseDeleteByKey<T>(ID, columstr, ref tranlist);  
        }


        public int Update(T model)
        {
            return idrive.Update<T>(model);
        }
        public int Update(T model, ref  List<TransactionHelp> tranlist)
        {
            return idrive.Update<T>(model, ref tranlist);  
        }

        public List<T> FindAll(T model) {
            return idrive.FindAll(model);
        
        }

        public T FindByID(T model) {

            return idrive.FindByID<T>(model);
        }

        public List<T> FindByProperty(T model) { 
        
            return  idrive.FindByProperty<T>(model);
                  
        }

        public List<T> FindLikeProperty(T model) {
            return idrive.FindLikeProperty<T>(model);
        }

        public List<T> ExecutiveSQL(string sql, SqlParameter[] paras=null) {

            return idrive.ExecutiveSQL<T>(sql, paras);
        
        }


  
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <param name="order"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
         public  List<T> SearchSqLFor_Page(string predicate, int page, int page_size, string order, string asc,out int total) 
        {
            return idrive.SearchSqLFor_Page<T>(predicate, page, page_size, order, asc,out total);
        }


        public IQUeryable<T> GetAllQUeryable() 
        {
            return idrive.GetAllQUeryable<T>();
        }

    

    }
}

using System;
namespace MT.Orm.CRUDMethod.SqlServer
{
    interface ICRUD
    {
        int Delete<T>(T model) where T : class;
        System.Collections.Generic.List<T> ExecutiveSQL<T>(string cmdText, System.Data.SqlClient.SqlParameter[] paras) where T : class, new();
        System.Collections.Generic.List<T> FindAll<T>(T model) where T : class, new();
        System.Collections.Generic.List<T> FindByID<T>(T model) where T : class, new();
        System.Collections.Generic.List<T> FindLikeProperty<T>(T model) where T : class, new();
        void GenerateBuildTable(System.Collections.Generic.List<Type> modeltype);
        int Insert<T>(T model) where T : class;
        bool IsConnectionSuccess();
        int Update<T>(T model) where T : class;
    }
}

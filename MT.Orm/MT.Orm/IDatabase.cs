using System.Collections.Generic;
using System.Data.SqlClient;

namespace MT.Orm
{
    public interface IDatabase
    {
        List<T> ExecutiveSQL<T>(string cmdText, params SqlParameter[] paras ) where T : class, new();
        int TransactionExecuteSqlCommand(ref  List<TransactionHelp> tranlist, string commandText, params SqlParameter[] commandParameters);

        int ExecuteSqlCommand(string cmdText, params SqlParameter[] paras);
         
    }

}

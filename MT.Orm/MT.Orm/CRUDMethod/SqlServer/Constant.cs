using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Orm.CRUDMethod.SqlServer
{
   
       public static class Constant
       {
           public static readonly string InnerSql = "INSERT INTO {0} ({1}) VALUES ({2})";

           public static readonly string UptadeSql = "UPDATE  {0} SET {1} WHERE 1=1 AND {2} ";

           public static readonly string DeleteSql = "  DELETE {0}  WHERE 1=1 {1}";

           public static readonly string FindByID = "SELECT  *  FROM  {0}   WHERE 1=1 AND  {1}";

           public static readonly string FindByProperty = "SELECT *  FROM  {0} WHERE 1=1 {1} ";


       
    }
}

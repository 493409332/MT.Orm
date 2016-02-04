using Complex.Sqlserver;
using MT.Common.Utility.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MT.Orm.Sqlserver
{
    public abstract class DbCheckHelper
    {
        //数据库连接字符串(web.config来配置)
        //<add key="ConnectionString" value="server=127.0.0.1;database=DATABASE;uid=sa;pwd=" />  
        //protected static string connectionString = ConfigurationManager.ConnectionStrings["SQLServerContext"].ConnectionString;//ConfigurationSettings.AppSettings["ConnectionString"];
        public DbCheckHelper()
        {
        }
        public static ArrayList ColumnsToAdd
        {
            get;
            set;
        }
        #region 公用方法
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entityColumnInfo">实体对应的字段信息</param>
        /// <param name="tableName">表名</param>
        /// <param name="overideOld">是否根据实体的字段属性直接覆盖到数据库</param>
        /// <returns></returns>
        public static ResultType DatabaseCheck(Dictionary<string, string[]> entityColumnInfo, string tableName, bool overideOld, string connectionString)
        {
            ResultType checkResult = ResultType.CHECK_FALSE;
            string[] connectionStringSpilt = connectionString.Split(';');
            List<string> tempList = new List<string>();
            string tempConnectionString;
            string replacePartStr = "";
            foreach (var item in connectionStringSpilt)
            {
                if (item.ToLower().TrimStart().StartsWith("database"))
                {
                    replacePartStr = item.Substring(item.IndexOf('=') + 1);
                    string replacePart = item.Replace(replacePartStr, "master");
                    tempList.Add(replacePart);
                }
                else
                {
                    tempList.Add(item);
                }
            }
            tempConnectionString = tempList.ToArray().Join(";");
            using (SqlConnection connection = new SqlConnection(tempConnectionString))
            {
                string isExistSQL = "SELECT 1 FROM sysdatabases WHERE NAME=N'" + replacePartStr + "'";
                using (SqlCommand cmd = new SqlCommand(isExistSQL, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = Convert.ToInt32(cmd.ExecuteScalar());

                        //如果数据库存在
                        if (int.Parse(rows.ToString()) > 0)
                        {
                            //Done：此处应该是验证实体和数据库是否对应
                            string getDBColumnsInfoSql = "SELECT co.name as ColumnName,co.isnullable as IsNullable,"
                                + " co.length as MaxLength,tb.name as TableName,ty.name as TypeName"
                                + " FROM syscolumns co,sys.tables tb,systypes ty"
                                + " WHERE  co.id= tb.object_id"
                                + " and co.xusertype = ty.xusertype"
                                + " and tb.name = '" + tableName + "'";
                            DataTable dt = new DataTable();
                            SqlConnection conn = new SqlConnection(connectionString);
                            try
                            {
                                SqlDataAdapter da = new SqlDataAdapter(getDBColumnsInfoSql, conn);
                                da.Fill(dt);           //将数据填充到指定的DataTable
                            }
                            catch (SqlException E)
                            {
                                throw E;
                            }
                            // Dictionary<字段名, string[字段类型,长度,是否为空]> entityColumnInfo
                            ArrayList sqlToChangeTablePropList = new ArrayList();
                            ArrayList sqlToAddNullableColumnList = new ArrayList();
                            ArrayList sqlToCopyColumnDataList = new ArrayList();
                            ArrayList sqlToDropColumnList = new ArrayList();
                            ArrayList sqlToRenameColumnList = new ArrayList();

                            ArrayList dropConSql = new ArrayList();
                            StringBuilder addConSql = new StringBuilder();
                            String dropPKSQL = "";
                            if (dt.Rows.Count > 0)
                            {
                                //如果查到了表信息，遍历
                                foreach (DataRow dr in dt.Rows)
                                {
                                    string columnName = dr["ColumnName"].SafeToString();
                                    string isNullable = dr["IsNullable"].SafeToString();
                                    string typeName = dr["TypeName"].SafeToString();
                                    string maxLength = dr["MaxLength"].SafeToString();
                                    if (entityColumnInfo.ContainsKey(columnName))
                                    {
                                        #region 实体和数据库中都有该字段,判断字段类型等是否相同

                                        if (!entityColumnInfo[columnName][0].SafeToString().ToLower().Equals(typeName)
                                            //|| !entityColumnInfo[columnName][1].SafeToString().Equals(maxLength)
                                            || entityColumnInfo[columnName][2].SafeToString().Equals(isNullable))
                                        {
                                            //如果覆盖数据库参数为True,修改数据库对应字段的类型
                                            if (overideOld)
                                            {
                                                #region 非重建数据库方式

                                                //StringBuilder sqlToChangeTableProp = new StringBuilder();
                                                //StringBuilder TableProp = new StringBuilder();
                                                //StringBuilder sqlToAddNullableColumn = new StringBuilder();
                                                //StringBuilder sqlToCopyColumnData = new StringBuilder();
                                                //StringBuilder sqlToDropColumn = new StringBuilder();
                                                //StringBuilder sqlToRenameColumn = new StringBuilder();

                                                ////sqlToChangeTableProp.Append("declare @pkName varchar(50);"
                                                ////      + " SELECT @pkName=b.name from sysobjects b , syscolumns a , sys.tables c"
                                                ////      + " WHERE   c.name = '" + tableName + "' and a.name ='" + columnName + "' and c.object_id= b.parent_obj;"
                                                ////      + " if(@pkName!=null and @pkName!='')"
                                                ////      + " exec('alter table " + tableName + " drop constraint ' + @pkName);");
                                                //dropPKSQL = " SELECT b.name from sysobjects b , syscolumns a , sys.tables c"
                                                //    + " WHERE   c.name = '" + tableName + "' and c.object_id= b.parent_obj;";
                                                //sqlToChangeTableProp.Append("alter table " + tableName + " alter column " + columnName);
                                                //if (!entityColumnInfo[columnName][0].SafeToString().ToLower().Equals(typeName))
                                                //{
                                                //    TableProp.Append(" " + entityColumnInfo[columnName][0]);
                                                //}
                                                //else
                                                //{
                                                //    TableProp.Append(" " + typeName);
                                                //}
                                                //if (!entityColumnInfo[columnName][1].SafeToString().Equals(maxLength))
                                                //{
                                                //    if (entityColumnInfo[columnName][0].SafeToString().ToLower().Equals("varchar")
                                                //        || entityColumnInfo[columnName][0].SafeToString().ToLower().Equals("nvarchar"))
                                                //    {
                                                //        TableProp.Append(" " + "(" + (entityColumnInfo[columnName][1].IsNullOrEmpty() ? "200" : entityColumnInfo[columnName][1]) + ")");
                                                //    }
                                                //}
                                                //if (!entityColumnInfo[columnName][2].SafeToString().Equals(isNullable))
                                                //{
                                                //    //sqlToChangeTableProp.Append(" " + (entityColumnInfo[columnName][2].Equals("1") ? "null" : "not null"));
                                                //    ////sqlToChangeTablePropList.Add("declare @name varchar(50); select @name=b.name from sysobjects b , syscolumns a , sys.tables c where   c.name = '"+tableName+"' and a.name ='"+columnName+"' and c.object_id= b.parent_obj; exec('alter table "+tableName+" drop constraint ' + @name);");
                                                //    //dropConSql.Add("declare @pkName varchar(50);"
                                                //    //    + " SELECT @pkName=b.name from sysobjects b , syscolumns a , sys.tables c"
                                                //    //    + " WHERE   c.name = '"+ tableName + "' and a.name ='" + columnName + "' and c.object_id= b.parent_obj;"
                                                //    //    + " if(@pkName!=null and @pkName!='')"
                                                //    //    + " exec('alter table " + tableName + " drop constraint ' + @pkName);");
                                                //    String tempColumnName = columnName + DateTime.Now.ToLongValue();
                                                //    sqlToAddNullableColumn.Append("alter table " + tableName + " add  " + tempColumnName);
                                                //    sqlToAddNullableColumn.Append(TableProp);
                                                //    if (typeName.Equals("uniqueidentifier"))
                                                //    {
                                                //        sqlToAddNullableColumn.Append(" default(NEWID()) ");
                                                //    }
                                                //    else
                                                //    {
                                                //        sqlToAddNullableColumn.Append(" default(0) ");
                                                //    }
                                                //    sqlToAddNullableColumn.Append(" " + (entityColumnInfo[columnName][2].Equals("1") ? "not null" : " null"));
                                                //    //sqlToAddNullableColumn.Append("; go");
                                                //    sqlToCopyColumnData.Append(" insert into " + tableName + "(" + tempColumnName + ") select  " + columnName + " from " + tableName);
                                                //    sqlToDropColumn.Append(" alter table " + tableName + " drop column " + columnName);
                                                //    sqlToRenameColumn.Append("EXEC sp_rename '" + tableName + ".[" + tempColumnName + "]', '" + columnName + "', 'COLUMN' ");
                                                //}
                                                //else
                                                //{
                                                //    if (entityColumnInfo[columnName][3].SafeToString().Equals("1"))
                                                //    {
                                                //        TableProp.Append(" not null");
                                                //    }
                                                //    else
                                                //    {
                                                //        TableProp.Append(" " + (isNullable.Equals("1") ? "not null" : " null"));
                                                //    }
                                                //}
                                                //sqlToChangeTablePropList.Add(sqlToChangeTableProp.Append(TableProp));
                                                //if (entityColumnInfo[columnName][3].SafeToString().Equals("1"))
                                                //{
                                                //    sqlToChangeTablePropList.Add(" exec('alter table " + tableName + " add constraint PK_" + tableName + "_" + columnName + " primary key (" + columnName + ")')");
                                                //}
                                                //if (!sqlToAddNullableColumn.ToString().IsNullOrEmpty())
                                                //{
                                                //    sqlToAddNullableColumnList.Add(sqlToAddNullableColumn);
                                                //}
                                                //if (!sqlToCopyColumnData.ToString().IsNullOrEmpty())
                                                //{
                                                //    sqlToCopyColumnDataList.Add(sqlToCopyColumnData);
                                                //}
                                                //if (!sqlToDropColumn.ToString().IsNullOrEmpty())
                                                //{
                                                //    sqlToDropColumnList.Add(sqlToDropColumn);
                                                //}
                                                //if (!sqlToRenameColumn.ToString().IsNullOrEmpty())
                                                //{
                                                //    sqlToRenameColumnList.Add(sqlToRenameColumn);
                                                //}
                                                //checkResult = ResultType.CHECKOKCHANGED;
                                                #endregion

                                                #region 重建数据库方式

                                                checkResult = ResultType.DELETE_AND_REBUILD;
                                                #endregion


                                            }
                                            else
                                            {
                                                checkResult = ResultType.CHECK_FALSE;
                                            }
                                        }
                                        if (!entityColumnInfo[columnName][1].SafeToString().Equals(maxLength))
                                        {
                                            if (entityColumnInfo[columnName][0].SafeToString().ToLower().Equals(typeName))
                                            {
                                                if (typeName.Equals("varchar"))
                                                {
                                                    checkResult = ResultType.CHANGE_LENGTH;
                                                }
                                               
                                            }
                                        }
                                        //Done：这里从实体的字段List中移除已经检查过的
                                        entityColumnInfo.Remove(columnName);
                                        #endregion
                                    }
                                    else
                                    {
                                        //删除数据库中已经在实体中移除的字段
                                        //string sqlToChangeTableProp = "alter table " + tableName + " drop " + columnName;
                                        //sqlToChangeTablePropList.Add(sqlToChangeTableProp);
                                    }

                                }
                                //Done:添加实体中比数据库中多的字段
                                if (entityColumnInfo.Count > 0)
                                {
                                    foreach (var item in entityColumnInfo)
                                    {
                                        if (item.Value[0].ToLower().Equals("varchar") || item.Value[0].ToLower().Equals("nvarchar"))
                                        {
                                            string sqlAddColumnsToDb = "Alter table " + tableName + " add " + item.Key + " " + item.Value[0] + " (" + (item.Value[1].IsNullOrEmpty() ? "200" : item.Value[1]) + ") " + " default(0) " + (item.Value[2].Equals("1") ? "null" : " null");
                                            sqlToChangeTablePropList.Add(sqlAddColumnsToDb);
                                        }
                                        else
                                        {
                                            string sqlAddColumnsToDb = "";
                                            if (item.Value[0].ToLower().Equals("uniqueidentifier"))
                                            {
                                                sqlAddColumnsToDb = "Alter table " + tableName + " add " + item.Key + " " + item.Value[0] + " default(NEWID()) " + (item.Value[2].Equals("1") ? "null" : " null");
                                            }
                                            else if (item.Value[4].SafeToString().Equals("1"))
                                            {
                                                sqlAddColumnsToDb = "Alter table " + tableName + " add " + item.Key + " " + item.Value[0] + "  " + (item.Value[2].Equals("1") ? "null" : " null");
                                            }
                                            else
                                            {
                                                sqlAddColumnsToDb = "Alter table " + tableName + " add " + item.Key + " " + item.Value[0] + " default(0) " + (item.Value[2].Equals("1") ? "null" : " null");
                                            }
                                            sqlToChangeTablePropList.Add(sqlAddColumnsToDb);
                                        }
                                    }
                                    ColumnsToAdd = sqlToChangeTablePropList;
                                    checkResult = ResultType.ONLY_ADD_COLUMNS;
                                }
                                //SqlHelper.ExecuteSqlTran(sqlToAddNullableColumnList, connectionString, CommandType.Text);
                                //SqlHelper.ExecuteSqlTran(sqlToCopyColumnDataList, connectionString, CommandType.Text);

                                ////SqlHelper.ExecuteSqlTran(sqlToDropColumnList, connectionString, CommandType.Text);
                                ////SqlHelper.ExecuteSqlTran(sqlToRenameColumnList, connectionString, CommandType.Text);
                                //using (SqlConnection con = new SqlConnection(connectionString))
                                //{
                                //    con.Open();
                                //    SqlTransaction tx = con.BeginTransaction("conTransaction");
                                //    try
                                //    {
                                //        String pkName = SqlHelper.ExecuteScalar(tx, CommandType.Text, dropPKSQL).SafeToString();
                                //        if (!pkName.IsNullOrEmpty())
                                //        {
                                //            SqlHelper.ExecuteNonQuery(conn, CommandType.Text, " alter table " + tableName + " drop constraint " + pkName);
                                //        }
                                //        SqlHelper.ExecuteSqlTran(sqlToDropColumnList, connectionString, CommandType.Text);
                                //        SqlHelper.ExecuteSqlTran(sqlToRenameColumnList, connectionString, CommandType.Text);
                                //        SqlHelper.ExecuteSqlTran(sqlToChangeTablePropList, connectionString, CommandType.Text);
                                //    }
                                //    catch (Exception)
                                //    {
                                //        tx.Rollback();
                                //        con.Close();
                                //        throw;
                                //    }
                                //}
                                //using (SqlConnection con2 = new SqlConnection(connectionString))
                                //{
                                //    con2.Open();
                                //    SqlTransaction tx2 = con2.BeginTransaction("con2Transaction");
                                //    try
                                //    {
                                //        SqlHelper.ExecuteSqlTran(sqlToChangeTablePropList, connectionString, CommandType.Text, tx2);
                                //    }
                                //    catch (Exception)
                                //    {
                                //        tx2.Rollback();
                                //        con2.Close();
                                //        throw;
                                //    }
                                //}
                                return checkResult;
                            }
                            else
                            {
                                //TODO：如果在数据库没查到该表,此处应该建表
                                return ResultType.TABLE_NOT_EXIST;
                            }
                            //return rows;
                        }
                        else
                        {
                            string createTable = "CREATE DATABASE " + replacePartStr;
                            using (SqlCommand sql = new SqlCommand(createTable, connection))
                            {
                                try
                                {
                                    int result = sql.ExecuteNonQuery();
                                    return ResultType.CREATE_DATABASE_SUCCESS;
                                }
                                catch (Exception E)
                                {
                                    connection.Close();
                                    throw new Exception(E.Message);
                                }
                            }
                        }
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
            
        }
        #endregion
    }
}

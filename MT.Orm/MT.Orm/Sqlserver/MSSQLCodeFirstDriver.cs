using Complex.Sqlserver;
using MT.Common.Utility.Extension;
using MT.Orm.DBAttribute;
using MT.Orm.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MT.Orm.Sqlserver
{
    public class MSSQLCodeFirstDriver : BaseCodeFirstDriver
    {
        #region   父类属性
        public override string mtableCommentString
        {
            get
            {
                return "execute sp_addextendedproperty N'{0}', '{1}', N'user', N'dbo', N'table', N'{2}', NULL, NULL;";
            }
        }
        public override string mColumnCommentString
        {
            get
            {
                return "exec sp_addextendedproperty N'{0}', N'{1}', N'user', N'dbo', N'table', N'{2}', N'column', N'{3}';";
            }
        }
        public override string mRenameTableString
        {
            get
            {
                return "exec sp_rename '{0}','{1}'";
            }
        }
        public override DatabaseType mDatabaseType
        {
            get
            {
                return DatabaseType.Sqlserver;
            }
        }
        public override ArrayList sqlToAddColumnList
        {
            get
            {
                return sqlAddColumnList;
            }
        }
        #endregion
        private ArrayList sqlAddColumnList;
        public List<EntityInfoModel> entityColumnList = new List<EntityInfoModel>();

        #region 验证数据库和实体
        /// <summary>
        /// 验证数据库和实体
        /// </summary>
        /// <param name="tableName">表名</param>
        public override ResultType CheckDB(String tableName)
        {
            Boolean IsCreateDB;
            DataTable dt = getDBColumnInfo(tableName, out IsCreateDB);
            if (!IsCreateDB)
            {
                List<TempColumnModel> columnInfoList = GetEntities<TempColumnModel>(dt);
                List<TempColumnModel> entityColumnInfo = entityColumnList.Select(p => new TempColumnModel
                {
                    ColumnName = p.ColumnName,
                    TableName = p.TableName,
                    TypeName = p.TypeName,
                    IsNullable = p.IsNullable,
                    MaxLength = p.MaxLength
                }).ToList();
                sqlAddColumnList = new ArrayList();
                ResultType checkResult = ResultType.CHECK_FALSE;
                if (dt.Rows.Count > 0)
                {
                    //如果查到了表信息，遍历
                    foreach (TempColumnModel item in columnInfoList)
                    {
                        if (entityColumnInfo.Where(p => p.ColumnName.Equals(item.ColumnName)).Count() > 0)
                        {
                            TempColumnModel model = entityColumnInfo.Where(p => p.ColumnName.Equals(item.ColumnName)).ToList().FirstOrDefault();

                            #region 实体和数据库中都有该字段,判断字段类型等是否相同
                            if (!model.TypeName.ToLower().Equals(item.TypeName)
                               || model.IsNullable.Equals(item.IsNullable))
                            {
                                //重建数据库
                                checkResult = ResultType.DELETE_AND_REBUILD;
                            }
                            //else if (!model.MaxLength.Equals(item.MaxLength.SafeToString()))
                            //{
                            //    if (model.TypeName.Equals("varchar"))
                            //    {

                            //    }
                            //}
                            //Done：这里从实体的字段List中移除已经检查过的
                            entityColumnInfo.Remove(model);
                            #endregion
                        }
                        else
                        {
                            //删除数据库中已经在实体中移除的字段,此处保留不动
                        }

                    }
                    //Done:添加实体中比数据库中多的字段
                    if (entityColumnInfo.Count() > 0)
                    {
                        foreach (var item in entityColumnInfo)
                        {
                            String sqlToAddColumn = "Alter table {0} add {1} {2} {3} {4}{5}";
                            sqlToAddColumn = String.Format(sqlToAddColumn,
                                item.TableName,
                                item.ColumnName,
                                item.TypeName,
                                item.TypeName.ToLower().Equals("varchar") ? ("(" + (item.MaxLength.IsNullOrEmpty() ? "200" : item.MaxLength) + ")") : "",
                                item.TypeName.ToLower().Equals("uniqueidentifier") ? "default(NEWID())" : "default(0)",
                                item.IsNullable.Equals("1") ? "null" : " null"
                                );
                            sqlToAddColumnList.Add(sqlToAddColumn);
                            checkResult = ResultType.ONLY_ADD_COLUMNS;
                        }
                    }
                    return checkResult;
                }
                else
                {
                    //TODO：如果在数据库没查到该表,此处应该建表
                    return ResultType.TABLE_NOT_EXIST;
                }
            }
            return ResultType.CREATE_DATABASE_SUCCESS;

        }

        public override void CheckPK(string tableName, List<EntityInfoModel> entityColumnList)
        {
            if (!entityColumnList.Exists(p => p.Primary.Equals("1")))
            {
                ArrayList delAddPK = new ArrayList();
                //实体中没有主键，删除数据库中主键
                delAddPK.Add(" declare @pkName varchar(50);"
                                 + " SELECT @pkName=name from sys.key_constraints where parent_object_id=object_id('" + tableName + "') and type='PK';"
                                 + " if(@pkName!=null and @pkName!='')"
                                 + " exec('alter table " + tableName + " drop constraint ' + @pkName);");
                //判断是否有名称为ID的列
                EntityInfoModel quer = entityColumnList.Where(p => p.ColumnName.ToUpper().Equals("ID")).FirstOrDefault();
                if (quer.IsNotNull())
                {
                    delAddPK.Add(" if not exists (SELECT 1 from sys.key_constraints where parent_object_id=object_id('" + tableName + "') and type='PK')"
                        + " exec('alter table " + tableName + " add constraint PK_" + tableName + "_ID" + " primary key (" + quer.ColumnName + ")')");
                }
                try
                {
                    SqlHelper.ExecuteSqlTran(delAddPK, ConnectionString, CommandType.Text);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            };
        }
        #endregion

        #region 连接字符串数据库切换至master
        /// <summary>
        /// 连接字符串数据库切换到master
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<String, String> getMasterConnString()
        {
            //CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;
            //int databaseleng = Compare.IndexOf(ConnectionString, "database", CompareOptions.IgnoreCase);
            //int semicolonleng = ConnectionString.IndexOf(";", databaseleng + 8);
            //String repstr = ConnectionString.Substring(databaseleng, semicolonleng - databaseleng);
            //return new KeyValuePair<String, String>(ConnectionString.Replace(repstr, "database=master"), repstr);
            String[] connectionStringSpilt = ConnectionString.Split(';');
            List<String> tempList = new List<String>();
            String replacePartStr = String.Empty;
            foreach (var item in connectionStringSpilt)
            {
                if (item.ToLower().TrimStart().StartsWith("database"))
                {
                    replacePartStr = item.Substring(item.IndexOf('=') + 1);
                    String replacePart = item.Replace(replacePartStr, "master");
                    tempList.Add(replacePart);
                }
                else
                {
                    tempList.Add(item);
                }
            }
            return new KeyValuePair<String, String>(tempList.ToArray().Join(";"), replacePartStr);
        }
        #endregion

        #region 获取数据库内实体信息
        /// <summary>
        /// 获取数据库内实体信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>数据库字段信息</returns>
        private DataTable getDBColumnInfo(String tableName, out Boolean isCreateDb)
        {
            KeyValuePair<String, String> connStrAndDBName = getMasterConnString();
            DataTable dt = new DataTable();
            Boolean toCreateDB = true;
            using (SqlConnection connection = new SqlConnection(connStrAndDBName.Key))
            {
                string isExistSQL = "SELECT 1 FROM sysdatabases WHERE NAME=N'" + connStrAndDBName.Value + "'";
                using (SqlCommand cmd = new SqlCommand(isExistSQL, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = Convert.ToInt32(cmd.ExecuteScalar());

                        //如果数据库存在
                        if (int.Parse(rows.ToString()) > 0)
                        {
                            toCreateDB = false;
                            //Done：此处应该是验证实体和数据库是否对应
                            string getDBColumnsInfoSql = "SELECT co.name as ColumnName,co.isnullable as IsNullable,"
                                + " co.length as MaxLength,tb.name as TableName,ty.name as TypeName"
                                + " FROM syscolumns co,sys.tables tb,systypes ty"
                                + " WHERE  co.id= tb.object_id"
                                + " and co.xusertype = ty.xusertype"
                                + " and tb.name = '" + tableName + "'";
                            SqlConnection conn = new SqlConnection(ConnectionString);
                            try
                            {
                                SqlDataAdapter da = new SqlDataAdapter(getDBColumnsInfoSql, conn);
                                da.Fill(dt);           //将数据填充到指定的DataTable

                            }
                            catch (SqlException E)
                            {
                                throw E;
                            }
                        }
                        else
                        {
                            String createTable = "CREATE DATABASE " + connStrAndDBName.Value;
                            using (SqlCommand sql = new SqlCommand(createTable, connection))
                            {
                                try
                                {
                                    int result = sql.ExecuteNonQuery();
                                    toCreateDB = true;
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
            isCreateDb = toCreateDB;
            return dt;
        }
        #endregion

        #region   C#类型转换成DB类型
        public SqlDbType CastDbType(Type cSharpType)
        {
            return (SqlDbType)CastToDbType(cSharpType, DatabaseType.Sqlserver);
        }
        #endregion



    }
}


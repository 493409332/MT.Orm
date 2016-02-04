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
    public class NewSqlserverDrive : BaseDBDriver
    {
        //public string ConnectionString
        //{
        //    get;
        //    set;
        //}
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
        BaseDBDriver bdb = new BaseDBDriver();
       

        #region 取实体的表特性
        ///// <summary>
        ///// 取实体的表特性
        ///// </summary>
        ///// <param name="type">实体的type</param>
        ///// <param name="tableName"></param>
        ///// <param name="tableComment"></param>
        //private static void GetTableAttribute(Type type, ref string tableName, ref string tableComment)
        //{
        //    object[] objs = type.GetCustomAttributes(typeof(TableAttribute), true);
        //    foreach (object obj in objs)
        //    {
        //        TableAttribute attr = obj as TableAttribute;
        //        if (attr != null)
        //        {

        //            tableName = attr.TableName;
        //            tableComment = attr.TableComment;
        //            break;
        //        }
        //    }
        //    if (tableName.IsNullOrEmpty())
        //    {
        //        tableName = type.Name;
        //    }
        //}
        #endregion

        #region 取实体的列特性
        /// <summary>
        /// 取实体的列特性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propInfo"></param>
        /// <returns>Dictionary<列字符串,注释></returns>
        private Dictionary<String, String> GetColumnAttribute(String tableName, PropertyInfo propInfo)
        {
            String commentStr = String.Empty;
            String oneColumnStr = String.Empty;
            Dictionary<String, String> columnInfoDic = new Dictionary<String, String>();

            object objAttrs = propInfo.GetCustomAttribute(typeof(ColumnAttribute), true);
            if (objAttrs != null)
            {
                #region 有列特性
                ColumnAttribute attr = objAttrs as ColumnAttribute;
                if (attr != null)
                {
                    //没配置映射字段名，默认使用属性名
                    if (attr.ColumnName.IsNullOrEmpty())
                    {
                        attr.ColumnName = propInfo.Name;
                    }
                    commentStr = String.Format("exec sp_addextendedproperty N'{0}', N'{1}', N'user', N'dbo', N'table', N'{2}', N'column', N'{3}'", Guid.NewGuid(), attr.Description, tableName, attr.ColumnName);

                    oneColumnStr = attr.ColumnName + " " + CastToDbType(propInfo.PropertyType).ToString();
                    String typeName = propInfo.PropertyType.Name.ToString().ToLower();

                    if (typeName.Equals("char") || typeName.Equals("string"))
                    {
                        oneColumnStr += attr.MaxLength.IsNullOrEmpty() ? "(200)" : " (" + attr.MaxLength + ")";
                    }
                    oneColumnStr += (attr.Unique ? " unique" : "")
                                     + (attr.Identity ? " identity (1,1) " : "")
                                     + (attr.Primary ? "  primary key" : "")
                                     + (attr.NotNull ? " not null" : " null");

                    entityColumnList.Add(new EntityInfoModel()
                    {
                        TableName = tableName,
                        ColumnName = attr.ColumnName,
                        IsNullable = attr.NotNull ? "1" : "0",
                        TypeName = CastToDbType(propInfo.PropertyType).ToString().ToLower(),
                        MaxLength = attr.MaxLength,
                        Primary = attr.Primary ? "1" : "0",
                        Identity = attr.Identity ? "1" : "0"
                    });
                }
                #endregion
            }
            else
            {
                #region 无列特性
                oneColumnStr = propInfo.Name + " " + CastToDbType(propInfo.PropertyType).ToString();
                //给varchar默认的长度
                if (propInfo.PropertyType.Name.ToString().ToLower().Equals("string"))
                {
                    oneColumnStr += "(200)";
                }
                entityColumnList.Add(new EntityInfoModel()
                   {
                       TableName = tableName,
                       ColumnName = propInfo.Name,
                       IsNullable = "0",
                       TypeName = CastToDbType(propInfo.PropertyType).ToString().ToLower(),
                       MaxLength = null,
                       Primary = "0",
                       Identity = "0"
                   });
                #endregion
            }
            columnInfoDic.Add(oneColumnStr, commentStr);
            return columnInfoDic;
        }
        #endregion

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
        /// <summary>
        /// C#类型转换成DB类型
        /// </summary>
        /// <param name="cSharpType">C#类型</param>
        /// <returns>DB类型</returns>
        public static SqlDbType CastToDbType(Type cSharpType)
        {
            //Nullable类型取原类型
            if (cSharpType.Name.ToString().Equals("Nullable`1"))
            {

                cSharpType = cSharpType.GetGenericArguments()[0];

            }
            if (cSharpType.IsEnum)
            {
                cSharpType = typeof(Int32);
            }
            SqlDbType result = new SqlDbType();
            switch (cSharpType.Name.ToString())
            {
                case "Int32":
                    result = SqlDbType.Int;
                    break;
                case "String":
                    result = SqlDbType.VarChar;
                    break;
                case "Boolean":
                    result = SqlDbType.Bit;
                    break;
                case "DateTime":
                    result = SqlDbType.DateTime;
                    break;
                case "Decimal":
                    result = SqlDbType.Decimal;
                    break;
                case "Double":
                    result = SqlDbType.Float;
                    break;
                case "Int16":
                    result = SqlDbType.SmallInt;
                    break;
                case "Int64":
                    result = SqlDbType.BigInt;
                    break;
                case "Object":
                    result = SqlDbType.Binary;
                    break;
                case "Single":
                    result = SqlDbType.Real;
                    break;
                case "Guid":
                    result = SqlDbType.UniqueIdentifier;
                    break;
                case "Char":
                    result = SqlDbType.Char;
                    break;

            }
            return result;
        }

        #endregion

        #region   DataTable转实体 通用方法
        /// <summary>
        /// DataTable转实体 通用方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> GetEntities<T>(DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                }
                entities.Add(entity);
            }
            return entities;
        }
        #endregion

        /// <summary>
        /// 反射取类上的属性
        /// </summary>
        /// <param name="types"></param>
        //public void GenerateBuildTable(List<Type> types)
        //{
        //    foreach (var type in types)
        //    {
        //        String tableName = String.Empty;
        //        String tableComment = String.Empty;
        //        //bdb.GetTableAttribute(type, ref tableName, ref tableComment);
        //        String createTableStr = "CREATE TABLE {0} ({1})";

        //        PropertyInfo[] propInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //        ArrayList columnStrList = new ArrayList();
        //        ArrayList commentList = new ArrayList();
        //        foreach (var propInfo in propInfos)
        //        {
        //            Dictionary<String, String> columnInfoDic = GetColumnAttribute(tableName, propInfo);
        //            columnStrList.Add(columnInfoDic.Keys.FirstOrDefault());
        //            commentList.Add(columnInfoDic.Values.FirstOrDefault());
        //        }
        //        if (columnStrList.Count > 0)
        //        {
        //            String columnStr = columnStrList.ToArray().Join(",");
        //            createTableStr = String.Format(createTableStr, tableName, columnStr);
        //            commentList.Add(String.Format("execute sp_addextendedproperty N'{0}', '{1}', N'user', N'dbo', N'table', N'{2}', NULL, NULL;", Guid.NewGuid(), tableComment, tableName));
        //            ArrayList finalSqls = new ArrayList();
        //            switch (CheckDB(tableName))
        //            {
        //                case ResultType.CREATE_DATABASE_SUCCESS:
        //                case ResultType.TABLE_NOT_EXIST:
        //                    finalSqls.Add(createTableStr);
        //                    finalSqls.AddRange(commentList);
        //                    break;
        //                case ResultType.CHECK_FALSE:
        //                    break;
        //                case ResultType.DELETE_AND_REBUILD:
        //                    //重建之前备份原数据库
        //                    finalSqls.Add("exec sp_rename '" + tableName + "','" + tableName + "备份" + DateTime.Now.ToLongValue() + "'");
        //                    finalSqls.Add(createTableStr);
        //                    break;
        //                case ResultType.ONLY_ADD_COLUMNS:
        //                    finalSqls.AddRange(sqlToAddColumnList);
        //                    break;
        //            };
        //            ExcuteSqls(finalSqls);
        //            //TODO： 验证主键个数
        //            if (!entityColumnList.Exists(p => p.Primary.Equals("1")))
        //            {
        //                ArrayList delAddPK = new ArrayList();
        //                //实体中没有主键，删除数据库中主键
        //                delAddPK.Add(" declare @pkName varchar(50);"
        //                                 + " SELECT @pkName=name from sys.key_constraints where parent_object_id=object_id('" + tableName + "') and type='PK';"
        //                                 + " if(@pkName!=null and @pkName!='')"
        //                                 + " exec('alter table " + tableName + " drop constraint ' + @pkName);");
        //                //判断是否有名称为ID的列
        //                EntityInfoModel quer = entityColumnList.Where(p => p.ColumnName.ToUpper().Equals("ID")).FirstOrDefault();
        //                if (quer.IsNotNull())
        //                {
        //                    delAddPK.Add(" if not exists (SELECT 1 from sys.key_constraints where parent_object_id=object_id('" + tableName + "') and type='PK')"
        //                        + " exec('alter table " + tableName + " add constraint PK_" + tableName + "_ID" + " primary key (" + quer.ColumnName + ")')");
        //                }
        //                try
        //                {
        //                    SqlHelper.ExecuteSqlTran(delAddPK, ConnectionString, CommandType.Text);
        //                }
        //                catch (Exception e)
        //                {
        //                    throw new Exception(e.Message);
        //                }
        //            };
        //        }
        //        entityColumnList.Clear(); ;
        //    }
        //}
    }
}


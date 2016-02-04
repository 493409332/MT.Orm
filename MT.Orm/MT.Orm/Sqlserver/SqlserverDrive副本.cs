using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MT.Common.Utility.Extension;
using MT.Orm.DBAttribute;
using System.Collections;

using Complex.Sqlserver;
using System.Globalization;
using MT.Orm;

namespace MT.Orm.Sqlserver
{
    public class SqlserverDrive副本 : IDrive
    {

        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// 反射取类上的属性
        /// </summary>
        /// <param name="types"></param>
        public void GenerateBuildTable(List<Type> types)
        {
            ArrayList finalToExecSql = new ArrayList();
            foreach (var type in types)
            {
                List<string> listColumnStr = new List<string>();

                StringBuilder sqlStr = new StringBuilder();
                StringBuilder addColumnComment = new StringBuilder();
                StringBuilder sqlToAddPK = new StringBuilder();

                ArrayList tableCommentCollection = new ArrayList();

                Dictionary<string, string[]> columnAndTypeListForCompare = new Dictionary<string, string[]>();

                string tableName = String.Empty;
                string tableComment = String.Empty;
                //取类上的特性
                object[] objs = type.GetCustomAttributes(typeof(TableAttribute), true);
                foreach (object obj in objs)
                {
                    TableAttribute attr = obj as TableAttribute;
                    if (attr != null)
                    {

                        tableName = attr.TableName;
                        tableComment = attr.TableComment;
                        break;
                    }
                }
                if (tableName.IsNullOrEmpty())
                {
                    tableName = type.Name;
                }
                // Console.WriteLine(string.Format("实体对应的表名:{0} ", tableName) + string.Format("表注释:{0} ", tableComment));
                sqlStr.Append("create table" + " " + tableName + "(");

                //取属性上的自定义特性
                foreach (PropertyInfo propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object objAttrs = propInfo.GetCustomAttribute(typeof(ColumnAttribute), true);
                    if (objAttrs != null)
                    {
                        #region 有列特性
                        ColumnAttribute attr = objAttrs as ColumnAttribute;
                        if (attr != null)
                        {
                            if (attr.ColumnName.IsNullOrEmpty())
                            {
                                attr.ColumnName = propInfo.Name;
                            }
                            addColumnComment.Append("\r\n exec sp_addextendedproperty N'" + Guid.NewGuid() + "', N'" + attr.Description + "', N'user', N'dbo', N'table', N'" + tableName + "', N'column', N'" + attr.ColumnName + "';");
                            tableCommentCollection.Add("exec sp_addextendedproperty N'" + Guid.NewGuid() + "', N'" + attr.Description + "', N'user', N'dbo', N'table', N'" + tableName + "', N'column', N'" + attr.ColumnName + "';");
                            String oneColumnStr = attr.ColumnName + " " + SqlTypeStringToSqlType(propInfo.PropertyType).ToString();
                            string maxLengthStr = attr.MaxLength.IsNullOrEmpty() ? "(200)" : " (" + attr.MaxLength + ")";
                            if (propInfo.PropertyType.Name.ToString().ToLower().Equals("char") || propInfo.PropertyType.Name.ToString().ToLower().Equals("string"))
                            {
                                oneColumnStr += maxLengthStr;
                            }
                            if (attr.Unique)
                            {
                                oneColumnStr += " unique";
                            }
                            if (attr.Identity)
                            {
                                oneColumnStr += " identity (1,1) ";
                            }
                            if (attr.Primary)
                            {
                                oneColumnStr += "  primary key";
                                attr.NotNull = true;
                            }
                            if (attr.NotNull)
                            {
                                oneColumnStr += " not null";
                            }
                            else
                            {
                                oneColumnStr += " null";
                            }
                            listColumnStr.Add(oneColumnStr); //列名
                            if (attr.MaxLength.IsNullOrEmpty())
                            {
                                if (propInfo.PropertyType.Name.ToString().ToLower().Equals("bool")
                                    || propInfo.PropertyType.Name.ToString().ToLower().Equals("boolean"))
                                {
                                    attr.MaxLength = "1";
                                }
                                else
                                {
                                    attr.MaxLength = "200";
                                }
                            }
                            columnAndTypeListForCompare.Add(attr.ColumnName,
                                new string[] { 
                                    SqlTypeStringToSqlType(propInfo.PropertyType).ToString().ToLower(),
                                    attr.MaxLength,
                                    attr.NotNull ? "1" : "0", 
                                    attr.Primary ? "1" : "0", 
                                    attr.Identity ? "1" : "0" 
                                });
                        }
                        #endregion
                    }
                    else
                    {
                        #region 无列特性
                        String oneColumnStr = propInfo.Name + " " + SqlTypeStringToSqlType(propInfo.PropertyType).ToString();
                        //给varchar默认的长度
                        if (propInfo.PropertyType.Name.ToString().ToLower().Equals("string"))
                        {
                            oneColumnStr += "(200)";
                        }
                        columnAndTypeListForCompare.Add(propInfo.Name, new string[] { SqlTypeStringToSqlType(propInfo.PropertyType).ToString().ToLower(), null, "0", "0", "0" });
                        listColumnStr.Add(oneColumnStr);
                        #endregion
                    }
                }
                if (listColumnStr.Count > 0)
                {
                    string[] colmuns = listColumnStr.ToArray();
                    sqlStr.Append(colmuns.Join(",") + ")");
                    addColumnComment.Append("\r\n EXECUTE sp_addextendedproperty N'" + Guid.NewGuid() + "', '" + tableComment + "', N'user', N'dbo', N'table', N'" + tableName + "', NULL, NULL;");
                    tableCommentCollection.Add("EXECUTE sp_addextendedproperty N'" + Guid.NewGuid() + "', '" + tableComment + "', N'user', N'dbo', N'table', N'" + tableName + "', NULL, NULL;");
                    //tableCommentCollection.Add(addColumnComment);
                    //sqlStr.Append(addColumnComment);
                    Console.WriteLine(sqlStr);  

                    //TODO: 根据实体和数据库查询到的表结构，分别生成 Dictionary<字段名, string[字段类型,长度,是否为空]> 做比对  
                    //columnAndTypeListForCompare
                    //try
                    //{
                    //    int pkCount = 0;
                    //    //TODO:验证主键个数，没有则设置ID为主键，没有ID则报错
                    //    foreach (var item in columnAndTypeListForCompare.Values)
                    //    {
                    //        //如果有字段主键设置为true
                    //        if (item[3].Equals("1"))
                    //        {
                    //            pkCount += 1;
                    //        }

                    //    }
                    //    if (pkCount == 0)
                    //    {
                    //        //var quer = columnAndTypeListForCompare.Where(p => p.Key.ToLower().Equals("id"));

                    //        if (columnAndTypeListForCompare.ContainsKey("ID"))
                    //        {
                    //            pkCount = 1;
                    //            columnAndTypeListForCompare["ID"][3] = "1";
                    //            sqlToAddPK.Append(" exec('alter table " + tableName + " add constraint PK_" + tableName + "_ID" + " primary key (ID)')");
                    //        }
                    //        else if (columnAndTypeListForCompare.ContainsKey("Id"))
                    //        {
                    //            pkCount = 1;
                    //            columnAndTypeListForCompare["Id"][3] = "1";
                    //            sqlToAddPK.Append(" exec('alter table " + tableName + " add constraint PK_" + tableName + "_ID" + " primary key (Id)')");
                    //        }
                    //        else if (columnAndTypeListForCompare.ContainsKey("iD"))
                    //        {
                    //            pkCount = 1;
                    //            sqlToAddPK.Append(" exec('alter table " + tableName + " add constraint PK_" + tableName + "_ID" + " primary key (iD)')");
                    //            columnAndTypeListForCompare["iD"][3] = "1";
                    //        }
                    //        else if (columnAndTypeListForCompare.ContainsKey("id"))
                    //        {
                    //            pkCount = 1;
                    //            sqlToAddPK.Append(" exec('alter table " + tableName + " add constraint PK_" + tableName + "_ID" + " primary key (id)')");
                    //            columnAndTypeListForCompare["id"][3] = "1";
                    //        }
                    //        else
                    //        {
                    //            throw new Exception("表'" + tableName + "'未设置主键列;");
                    //        }
                    //    }
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}
                    var result = DbCheckHelper.DatabaseCheck(columnAndTypeListForCompare, tableName, true, ConnectionString);
                    if (result == ResultType.TABLE_NOT_EXIST 
                        || result == ResultType.CREATE_DATABASE_SUCCESS 
                        || result == ResultType.DELETE_AND_REBUILD)
                    {
                        if (result == ResultType.DELETE_AND_REBUILD)
                        {
                            //TODO:此处重命名原表
                            finalToExecSql.Add("exec sp_rename '" + tableName + "','" + tableName + "备份" + DateTime.Now.ToLongValue() + "'");
                        }
                        finalToExecSql.Add(sqlStr.ToString());
                        //建表
                        //SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sqlStr.ToString());
                        //表注释
                        //SqlHelper.ExecuteSqlTran(tableCommentCollection, SqlHelper.GetConnString(), CommandType.Text);
                        finalToExecSql.AddRange(tableCommentCollection);
                        finalToExecSql.Add(sqlToAddPK.ToString());
                    }
                    if (result == ResultType.ONLY_ADD_COLUMNS)
                    {
                        finalToExecSql.AddRange(DbCheckHelper.ColumnsToAdd);
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                SqlHelper.ExecuteSqlTran(finalToExecSql, ConnectionString, CommandType.Text, transaction);
            }

        }


        #region   C#类型转换成DB类型

        private static SqlDbType SqlTypeStringToSqlType(Type cSharpType)
        {
            if (cSharpType.Name.ToString().Equals("Nullable`1"))
            {

                cSharpType = cSharpType.GetGenericArguments()[0];

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

            }
            return result;
        }

        #endregion

        private class TempColumnClass
        {
            public string key { get; set; }
            public string value { get; set; }
            public SqlDbType columnType { get; set; }
            public string maxLength { get; set; }
        }

        #region 取属性的值
        /// <summary>
        ///  字段和值的列表
        /// </summary>
        private static List<TempColumnClass> getColumnInfo<T>(T model)
        {
            List<TempColumnClass> columnInfo = new List<TempColumnClass>();
            TempColumnClass temp;
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                temp = new TempColumnClass();
                if (propertyInfo.GetValue(model, null).IsNull())
                {
                    continue;
                }
                else
                {
                    string propertyValue = string.Empty;
                    Type propertyType = propertyInfo.PropertyType;
                    //对于字符和datetime，要加上引号
                    if (propertyType == typeof(System.String) || propertyType == typeof(System.DateTime) || propertyType == typeof(System.DateTime?))
                    {
                        propertyValue = "'" + propertyInfo.GetValue(model, null).ToString() + "'";
                    }
                    else
                    {
                        propertyValue = propertyInfo.GetValue(model, null).ToString();
                    }
                    if (true)
                    {
                        object objAttr = propertyInfo.GetCustomAttribute(typeof(ColumnAttribute), true);
                        ColumnAttribute attr = objAttr as ColumnAttribute;
                        if (attr != null)
                        {
                            temp.maxLength = attr.MaxLength;
                        }
                    }
                    temp.key = propertyInfo.Name;
                    temp.value = propertyValue;
                    temp.columnType = SqlTypeStringToSqlType(propertyType);
                    columnInfo.Add(temp);
                }
            }
            return columnInfo;
        }
        #endregion



        #region IDrive 成员

        public bool IsConnectionSuccess()
        {
            //string[] connectionStringSpilt = ConnectionString.Split(';');
            //List<string> tempList = new List<string>();
            //string tempConnectionString;
            //string replacePartStr = "";
            //foreach ( var item in connectionStringSpilt )
            //{
            //    if ( item.ToLower().TrimStart().StartsWith("database") )
            //    {
            //        replacePartStr = item.Substring(item.IndexOf('=') + 1);
            //        string replacePart = item.Replace(replacePartStr, "master");
            //        tempList.Add(replacePart);
            //    }
            //    else
            //    {
            //        tempList.Add(item);
            //    }
            //}


            CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;

            int databaseleng = Compare.IndexOf(ConnectionString, "database", CompareOptions.IgnoreCase);

            int semicolonleng = ConnectionString.IndexOf(";", databaseleng + 8);

            string repstr = ConnectionString.Substring(databaseleng, semicolonleng - databaseleng);

            string replacePart = ConnectionString.Replace(repstr, "database=master");

            SqlConnection connection = new SqlConnection(replacePart);



            string error = null;
            bool? success = null;

            var thread = new Thread(() =>
            {
                try
                {
                    connection.Open();
                    connection.Close();

                    success = true;
                }
                catch (SqlException ex)
                {
                    success = false;
                    error = ex.Message;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            });

            thread.IsBackground = true;
            var sw = Stopwatch.StartNew();
            thread.Start();

            var timeout = TimeSpan.FromSeconds(3);
            while (sw.Elapsed < timeout)
            {
                thread.Join(TimeSpan.FromMilliseconds(200));
                if (success != null)
                {
                    break;
                }
            }
            sw.Stop();

            if (!success.HasValue && !(bool)success)
            {
                throw new Exception(error ?? "连接数据库超时，请检查输入的服务器是否正确。");
            }
            return success.HasValue && (bool)success;
        }


        public string GenerateBuildTable(Type modeltype)
        {
            throw new NotImplementedException();
        }




        #endregion





        
    

        #region IDrive 成员  CRUD

        /// <summary>
        /// 分页  
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        public List<T> SearchSqLFor_Page<T>(string predicate, int page, int page_size, string order, string asc, out int total) where T : class,new()
        {
            ///获取表名
            String TableName = GetTableName(typeof(T));
            string Sql = string.Format(" SELECT TOP {0}  * FROM ( SELECT *, row_number() OVER (ORDER BY {1} {2}) AS [row_number]   FROM [dbo].[{5}] AS [Extent1]  where IsDelete='False' and {3} )  AS [Extent1]    WHERE [Extent1].[row_number] > {4}   ORDER BY [Extent1].{1} {2}", page_size, order.Replace("'", "''"), asc.Replace("'", "''"), predicate, (page - 1) * page_size, TableName);
            string Sql2 = string.Format(" SELECT  COUNT(1) as total   FROM {0} ", TableName);
            DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, Sql.ToString(), null).Tables[0];
            object data2 = SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, Sql2);  

            try
            {
                total = (int)data2;
                return DtTolist.ToList<T>(data);
            }
            catch (Exception e)
            {
                total = 0;
                throw e;
            }
      
        }





        public int Insert<T>(T model) where T : class
        {
            if (model != null)
            {

                //验证并获取列属性
                Dictionary<string, object> valu = Verification(model, GetAttribute(model));
                //列名称
                string ColumnName = AssemblyName(valu);
                //列的值
                string values = AssemblyValue(valu);
                //参数
                SqlParameter[] paras = AssemblyParameter(valu);
                //表名
                String TableName = GetTableName(typeof(T));
                if (valu != null)
                {
                    try
                    {
                        string cmdText = string.Format(Constant.InnerSql, TableName, ColumnName, values);
                        return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, paras);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }

                }
            }
            return -1;


        }

        public int Delete<T>(T model) where T : class
        {
            Type type = typeof(T);
            List<SqlParameter> paras = new List<SqlParameter>();
            StringBuilder Where = new StringBuilder();
            Dictionary<string, ColumnAttribute> Col = GetAttribute(model);
            Dictionary<string, object> valu = Verification(model, Col);
            //表名
            String TableName = GetTableName(typeof(T));



            foreach (var item in valu.Keys)
            {
                dynamic obj = valu[item.ToString()];
                object v = obj.value;

                if (v != null)
                {
                    Where.AppendFormat(" and {0}=@{0}", item.ToString());

                    paras.Add(new SqlParameter("@" + item.ToString(), v));

                }

            }

            try
            {
                string cmdText = string.Format(Constant.DeleteSql, TableName, Where);
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, paras.ToArray());
            }
            catch (Exception)
            {
                return -1;
                throw;
            }
        }


        public int DeleteByKey<T>(T model) where T : class
        {
            Type type = typeof(T);
            Dictionary<string, ColumnAttribute> Col = GetAttribute(model);
            //主键拼装  ID=1 
            string PK = GetPK(model, Col);
            //表名
            String TableName = GetTableName(typeof(T));
            try
            {
                string cmdText = string.Format(Constant.DeleteSql, TableName, PK.ToString());
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Update<T>(T model) where T : class
        {
            if (model != null)
            {
                Type type = typeof(T);
                //属性
                Dictionary<string, ColumnAttribute> Attributes = GetAttribute(model);

                //验证并获取列属性
                Dictionary<string, object> valu = Verification(model, Attributes);
                //列名称
                string Set = GetSet(valu, Attributes);
                //主键拼装  ID=1 
                string PK = GetPK(model, Attributes);
                //参数
                SqlParameter[] paras = AssemblyParameter(valu);
                //表名
                String TableName = GetTableName(typeof(T));


                string cmdText = string.Format(Constant.UptadeSql, TableName, Set, PK);

                try
                {
                    //Console.WriteLine(cmdText.ToString());
                    //Console.Read();
                    return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText.ToString(), paras);
                }
                catch (Exception e)
                {

                    throw e;
                }



            }
            return -1;
        }

        public List<T> FindByID<T>(T model) where T : class,new()
        {
            if (model != null)
            {
                Type type = typeof(T);

                StringBuilder cmdText = new StringBuilder();
                object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
                TableAttribute Classattr = objs as TableAttribute;

                foreach (PropertyInfo propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object objAttrs = propInfo.GetCustomAttribute(typeof(ColumnAttribute), true);
                    ColumnAttribute attr = objAttrs as ColumnAttribute;
                    //寻找主键
                    if (attr != null && attr.ColumnName != null)
                    {

                        if (attr.Primary)
                        {
                            cmdText.AppendFormat(" select  *  From  {0}   where 1=1 and  {1}=@{1} ", Classattr.TableName, propInfo.Name.ToString());
                            SqlParameter paras = new SqlParameter("@" + propInfo.Name.ToString(), propInfo.GetValue(model, new object[0]));//  paras[0] = new SqlParameter("@" + propInfo.Name.ToString(), SqlTypeStringToSqlType(propInfo.PropertyType.FullName.ToString()));
                            try
                            {

                                DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), paras).Tables[0];


                                if (data.Rows.Count > 0)
                                {

                                    return DtTolist.ToList<T>(data);

                                }


                            }
                            catch (Exception e)
                            {

                                // return null;
                                throw e;
                            }

                            return null;
                        }
                    }
                }
            }

            return null;
        }

        public List<T> FindByProperty<T>(T model) where T : class,new()
        {
            if (model != null)
            {
                Type type = typeof(T);
                List<SqlParameter> paras = new List<SqlParameter>();
                StringBuilder Where = new StringBuilder();
                Dictionary<string, ColumnAttribute> Col = GetAttribute(model);
                Dictionary<string, object> valu = Verification(model, Col);
                //表名
                String TableName = GetTableName(typeof(T));



                foreach (var item in valu.Keys)
                {
                    dynamic obj = valu[item.ToString()];
                    object v = obj.value;

                    if (v != null)
                    {

                        Where.AppendFormat(" and {0}=@{0}", item.ToString());

                        paras.Add(new SqlParameter("@" + item.ToString(), v));

                    }

                }
                try
                {
                    string cmdText = string.Format(Constant.FindByProperty, TableName, Where);

                    DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), paras.ToArray()).Tables[0];


                    if (data.Rows.Count > 0)
                    {
                        return DtTolist.ToList<T>(data);

                    }
                }
                catch (Exception e)
                {

                    throw e;
                }


            }

            return null;
        }

        public List<T> FindAll<T>(T model) where T : class,new()
        {
            Type type = typeof(T);
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("Select * From {0}", Classattr.TableName);
            try
            {
                DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), null).Tables[0];
                if (data.Rows.Count > 0)
                {
                    List<T> l = DtTolist.ToList<T>(data);
                    //  Expression a = LinqWhere.ToLinqWhere<T>("{field: \"ID\",data: \"1\",op: \"eq\",Groups:{field: \"ID\",data: \"1\",op: \"eq\"}}");
                    return l;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }

        public List<T> FindLikeProperty<T>(T model) where T : class,new()
        {
            if (model != null)
            {
                Type type = typeof(T);
                List<SqlParameter> paras = new List<SqlParameter>();
                StringBuilder Where = new StringBuilder();
                Dictionary<string, ColumnAttribute> Col = GetAttribute(model);
                Dictionary<string, object> valu = Verification(model, Col);
                //表名
                String TableName = GetTableName(typeof(T));



                foreach (var item in valu.Keys)
                {
                    dynamic obj = valu[item.ToString()];
                    object v = obj.value;

                    if (v != null)
                    {

                        Where.AppendFormat(" and {0} LIKE '%@ {0} %'", item.ToString());

                        paras.Add(new SqlParameter("@" + item.ToString(), v));

                    }

                }
                try
                {
                    string cmdText = string.Format(Constant.FindByProperty, TableName, Where);

                    DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), paras.ToArray()).Tables[0];


                    if (data.Rows.Count > 0)
                    {
                        return DtTolist.ToList<T>(data);

                    }
                }
                catch (Exception e)
                {

                    throw e;
                }


            }

            return null;
        }


        public List<T> ExecutiveSQL<T>(string cmdText, SqlParameter[] paras) where T : class, new()
        {
            if (cmdText != null)
            {
                try
                {
                    DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), paras).Tables[0];
                    if (data.Rows.Count > 0)
                    {
                        return DtTolist.ToList<T>(data);

                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return null;

        }

        public List<T> ExecutiveSQL<T>(T model) where T : class,new()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region    验证Attribute表示的列属性
        /// <summary>
        /// 验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        public static Dictionary<string, object> Verification<T>(T model, Dictionary<string, ColumnAttribute> Col) where T : class
        {
            ///获取本方法的调用者
            StackTrace trace = new StackTrace();
            string A = trace.GetFrame(1).GetMethod().ToString();

            Dictionary<string, object> valu = new Dictionary<string, dynamic>();



            if (model != null)
            {

                Type type = typeof(T);

                object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
                TableAttribute Classattr = objs as TableAttribute;
                foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (item != null)
                    {
                        //如果此列有Attribute才验证

                        // trace.GetFrame(1).GetMethod().Name !=  typeof(SqlserverDrive).GetMethod("FindByProperty").Name


                        if (Col.ContainsKey(item.Name) && trace.GetFrame(1).GetMethod().ToString() != "System.Collections.Generic.List`1[T] FindByProperty[T](T)")
                        {

                            ColumnAttribute d = Col[item.Name];

                            //非空验证
                            if (d.NotNull)
                            {

                                if (item.GetValue(model, new object[0]) == null)
                                {
                                    throw new Exception(item.Name.ToString() + "列不能为空，请检查");
                                }
                            }

                            //主键验证
                            if (d.Primary && item.GetValue(model, new object[0]) == null && !d.Identity)
                            {

                                throw new Exception(item.Name.ToString() + "列为主键，值不允许为空");
                            }
                            string a = trace.GetFrame(1).GetMethod().ToString();
                            if (trace.GetFrame(1).GetMethod().ToString() != "Int32 Update[T](T)" && trace.GetFrame(1).GetMethod().ToString() != "Int32 Delete[T](T)")
                            {
                                //自增长验证
                                if (d.Identity && item.GetValue(model, new object[0]) != null)
                                {
                                    valu.Remove(item.Name.ToString());
                                    continue;

                                }
                                else if (d.Identity)
                                {
                                    valu.Remove(item.Name.ToString());
                                    continue;
                                }
                            }
                        }
                        //通过验证后
                        SqlDbType types = SqlTypeStringToSqlType(item.PropertyType);
                        object values = null;
                        if (item.GetValue(model, new object[0]) != null)
                        {
                            values = item.GetValue(model, new object[0]);
                        }
                        //value  值   type  数据类型  length  长度【目前length没有处理，明天加上】 
                        dynamic obj = new { value = values, type = types, length = 0 };
                        valu.Add(item.Name, obj);
                    }
                }

            }

            return valu;

        }
        #endregion


        #region    获取类属性
        /// <summary>
        /// 获取类属性Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<string, ColumnAttribute> GetAttribute<T>(T model) where T : class
        {
            Type type = typeof(T);
            Dictionary<string, ColumnAttribute> Col = new Dictionary<string, ColumnAttribute>();
            foreach (PropertyInfo propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object objAttrs = propInfo.GetCustomAttribute(typeof(ColumnAttribute), true);
                ColumnAttribute attr = objAttrs as ColumnAttribute;
                if (attr != null && attr.ColumnName != null)
                {
                    Col.Add(attr.ColumnName, attr);
                }

            }
            return Col;
        }
        #endregion

        #region 获取表名
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {

            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;

            return Classattr.TableName;



        }
        #endregion

        #region 拼装列名
        /// <summary>
        /// 拼装列名  name,name,name.....
        /// </summary>
        /// <param name="valu"></param>
        /// <returns></returns>
        public static string AssemblyName(Dictionary<string, object> valu)
        {
            StringBuilder ColumnNames = new StringBuilder();
            if (valu != null)
            {
                int i = 0;
                foreach (var item in valu.Keys)
                {
                    if (i <= 0)
                    {
                        ColumnNames.Append(item.ToString());
                    }
                    else
                    {
                        ColumnNames.Append(" , " + item.ToString());
                    }
                    i++;
                }
            }
            return ColumnNames.ToString();
        }
        #endregion

        #region 拼装值
        /// <summary>
        /// 拼装值  value,value,value.....
        /// </summary>
        /// <param name="valu"></param>
        /// <returns></returns>
        public static string AssemblyValue(Dictionary<string, object> valu)
        {
            int i = 0;
            StringBuilder value = new StringBuilder();
            foreach (var item in valu.Keys)
            {
                dynamic obj = valu[item.ToString()];
                object v = obj.value;
                if (i <= 0)
                {
                    value.Append("@" + item.ToString());
                }
                else
                {
                    value.Append(" , @" + item.ToString());
                }

                //paras[i] = new SqlParameter("@" + item.ToString(), obj.type);
                //paras[i].Value = v;
                i++;

            }
            return value.ToString();
        }
        #endregion

        #region SqlParameter
        /// <summary>
        /// SqlParameter
        /// </summary>
        /// <param name="valu"></param>
        /// <returns></returns>
        public static SqlParameter[] AssemblyParameter(Dictionary<string, object> valu)
        {
            int i = 0;
            SqlParameter[] paras = new SqlParameter[valu.Count];
            StringBuilder value = new StringBuilder();
            foreach (var item in valu.Keys)
            {
                dynamic obj = valu[item.ToString()];
                // object v = ;

                if (obj.value != null)
                {
                    paras[i] = new SqlParameter("@" + item.ToString(), obj.type);
                    paras[i].Value = obj.value;
                    i++;
                }

            }
            return paras;
        }
        #endregion

        #region  Update set
        /// <summary>
        /// GetSet
        /// </summary>
        /// <param name="valu"></param>
        /// <returns></returns>
        public static string GetSet(Dictionary<string, object> valu, Dictionary<string, ColumnAttribute> Attributes)
        {

            int i = 0;
            StringBuilder Set = new StringBuilder();
            // ColumnAttribute 
            string iden = Attributes.Values.Where(p => p.Identity).First().ColumnName;

            foreach (var item in valu.Keys)
            {

                dynamic obj = valu[item.ToString()];
                object v = obj.value;

                if (item.ToString() != iden && v != null)
                {

                    if (i <= 0)
                    {
                        Set.AppendFormat("{0}=@{0}", item.ToString());
                    }
                    else
                    {
                        Set.AppendFormat(",{0}=@{0}", item.ToString());
                    }
                }

                i++;

            }
            return Set.ToString();
        }
        #endregion

        #region  拼装主键
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        public static string GetPK<T>(T model, Dictionary<string, ColumnAttribute> Col) where T : class
        {
            Dictionary<string, object> valu = new Dictionary<string, dynamic>();
            string PK = " {0}={1} ";
            if (model != null)
            {
                Type type = typeof(T);

                object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
                TableAttribute Classattr = objs as TableAttribute;
                string d = Col.Values.Where(p => p.Identity).First().ColumnName;
                foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (item != null)
                    {
                        //   ColumnAttribute d = Col[item.Name];

                        //主键验证
                        if (item.Name.ToString().Equals(d))
                        {

                            return string.Format(PK, item.Name, item.GetValue(model, new object[0]).ToString());
                        }

                    }

                }
            }
            return null;
        }
        #endregion

        //public List<T> ExecutiveSQL<T>(string cmdText, SqlParameter[] paras) where T : class, new()
        //{
        //    throw new NotImplementedException();
        //}

        #region IDrive 成员


        //public List<T> Where<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new()
        //{

        //    IQUeryable<T> aaa = (IQUeryable<T>) new List<T>(3);
        //    aaa.Where(predicate);


        //    throw new NotImplementedException();
        //}

        public IQUeryable<T> GetAllQUeryable<T>() where T : class, new()
        {
            IQUeryable<T> quer = new RBaseQUeryable<T>();
            quer.Parsing = new Parsing { DBType = DatabaseType.Sqlserver };
            return quer;
        }

        public List<T> ToList<T>() where T : class, new()
        {
            return new List<T>();
        }

        public int Count<T>() where T : class, new()
        {
            return 0;
        }



        #endregion


        T IDrive.FindByID<T>(T model)
        {
            throw new NotImplementedException();
        }


     


           //List<TransactionHelp> test;
           //bool IsSucceed = Test(out test) > 0 && Test(out test) > 0 && Test(out test) > 0; 
           //Transaction(test, IsSucceed); 
 
        public int TransactionExecuteScalar(out  List<TransactionHelp> tranlist,string commandText, params SqlParameter[] commandParameters)
        {
            tranlist = new List<TransactionHelp>();
            int total = 0;
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();
            SqlTransaction transaction;
            transaction = connection.BeginTransaction("SampleTransaction");
            try
            {
                total = (int) SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, commandText, null);
            }
            catch ( Exception e )
            {
                transaction.Rollback();
                connection.Close();
                connection.Dispose();
                throw e;
            }
            TransactionHelp tran = new TransactionHelp();
            tran.Commit = transaction.Commit;
            tran.Rollback = transaction.Rollback;
            tran.DispostSL = connection;
            tranlist.Add(tran);
            return total;
        }

     
    }
    
}


using MT.Common.Utility.Extension;
using MT.Orm.DBAttribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MT.Orm.Sqlserver
{
    public class SqlserverDrive : IDrive, IDatabase
    {

        public string ConnectionString
        {
            get;
            set;
        }
        #region 数据库生成与验证
        /// <summary>
        /// 反射取类上的属性
        /// </summary>
        /// <param name="types"></param>
        public void GenerateBuildTable(List<Type> types)
        {
            MSSQLCodeFirstDriver ns = new MSSQLCodeFirstDriver();
            ns.GenerateBuildTable(types);
        }
        #endregion

        #region IDrive 成员

        public bool IsConnectionSuccess()
        { 
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
             
            total = (int)data2;
            return DtTolist.ToList<T>(data); 
        }

         
        public int Insert<T>(T model) where T : class
        {
            if (model != null)
            {

                //验证并获取列属性
                Dictionary<string, object> valu = Verification(model, GetAttribute(model));
                if (valu != null)
                {
                    //列名称
                    string ColumnName = AssemblyName(valu);
                    //列的值
                    string values = AssemblyValue(valu);
                    //参数
                    SqlParameter[] paras = AssemblyParameter(valu);
                    //表名
                    String TableName = GetTableName(typeof(T));
                     
                    string cmdText = string.Format(Constant.InnerSql, TableName, ColumnName, values);
                    return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, paras);


                }
            }
            return 0;


        }
        public int Insert<T>(T model,ref  List<TransactionHelp> tranlist) where T : class
        {
            if (model != null)
            {

                //验证并获取列属性
                Dictionary<string, object> valu = Verification(model, GetAttribute(model));
                if (valu != null)
                {
                    //列名称
                    string ColumnName = AssemblyName(valu);
                    //列的值
                    string values = AssemblyValue(valu);
                    //参数
                    SqlParameter[] paras = AssemblyParameter(valu);
                    //表名
                    String TableName = GetTableName(typeof(T));


                    string cmdText = string.Format(Constant.InnerSql, TableName, ColumnName, values);
                    //   return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, paras);

                    return TransactionExecuteSqlCommand(ref tranlist, cmdText, paras);
                     
                }
            } 
            return 0;


        }
        public int DeleteByModel<T>(T model) where T : class
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
            string cmdText = string.Format(Constant.DeleteSql, TableName, Where);
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, paras.ToArray());

        }
        public int DeleteByModel<T>(T model, ref  List<TransactionHelp> tranlist) where T : class
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
            string cmdText = string.Format(Constant.DeleteSql, TableName, Where);
           // return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, paras.ToArray());

            return TransactionExecuteSqlCommand(ref tranlist, cmdText, paras.ToArray());
        }

        public int DeleteByKey<T>(object ID) where T : class
        {
            Type type = typeof(T);
            Dictionary<string, ColumnAttribute> Col = GetAttribute(type);
            //主键拼装  ID=1 
            string PK = GetPK<T>(type, Col, ID);
            //表名
            String TableName = GetTableName(typeof(T));

            string cmdText = string.Format(Constant.DeleteSql, TableName, PK.ToString());
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, null);

        }
        public int DeleteByKey<T>(object ID, ref  List<TransactionHelp> tranlist) where T : class
        {
            Type type = typeof(T);
            Dictionary<string, ColumnAttribute> Col = GetAttribute(type);
            //主键拼装  ID=1 
            string PK = GetPK<T>(type, Col, ID);
            //表名
            String TableName = GetTableName(typeof(T));

            string cmdText = string.Format(Constant.DeleteSql, TableName, PK.ToString());
          //  return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, null);

            return TransactionExecuteSqlCommand(ref tranlist, cmdText, null);

        }

        public int FalseDeleteByKey<T>(object ID, string columstr ) where T : class
        {
            Type type = typeof(T);
            Dictionary<string, ColumnAttribute> Col = GetAttribute(type);
            //主键拼装  ID=1 
            string PK = GetPK<T>(type, Col, ID);
            if (PK == null  )
            {
                throw new Exception("无法确定实体的主键");
            }
            string IsDeleteStr = GetIsDelete<T>(columstr);
            if (  IsDeleteStr == null)
            {
                throw new Exception("无法确定实体的逻辑删除字段");
            }
            //表名
            String TableName = GetTableName(typeof(T));  
            string cmdText = string.Format(Constant.DeleteSql, TableName, PK.ToString());
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, null);

        }

        public int FalseDeleteByKey<T>(object ID, string columstr, ref  List<TransactionHelp> tranlist) where T : class
        {
            Type type = typeof(T);
            Dictionary<string, ColumnAttribute> Col = GetAttribute(type);
            //主键拼装  ID=1 
            string PK = GetPK<T>(type, Col, ID);
            if (PK == null)
            {
                throw new Exception("无法确定实体的主键");
            }
            string IsDeleteStr = GetIsDelete<T>(columstr);
            if (IsDeleteStr == null)
            {
                throw new Exception("无法确定实体的逻辑删除字段");
            }
            //表名
            String TableName = GetTableName(typeof(T));
            string cmdText = string.Format(Constant.DeleteSql, TableName, PK.ToString());
            //return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, null);

            return TransactionExecuteSqlCommand(ref tranlist, cmdText, null);

        }


        public static string GetIsDelete<T>(string columstr) where T : class
        {
            Type type = typeof(T);
            var columnprop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name == columstr).FirstOrDefault();
            string IsDeletestr = " {0}={1} ";
            return columnprop != null && columnprop.PropertyType == typeof(bool) ? string.Format(IsDeletestr, columstr, "true") : null; 
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
        public int Update<T>(T model, ref  List<TransactionHelp> tranlist) where T : class
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
                 
                //Console.WriteLine(cmdText.ToString());
                //Console.Read();
              //  return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText.ToString(), paras);
                return TransactionExecuteSqlCommand(ref tranlist, cmdText, paras);
                 
            }
            return 0;
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


        public List<T> ExecutiveSQL<T>(string cmdText,params SqlParameter[] paras) where T : class, new()
        {
            if (cmdText != null)
            {
                DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), paras).Tables[0];
                if (data.Rows.Count > 0)
                {
                    return DtTolist.ToList<T>(data); 
                } 
            }
            return null; 
        }
        public int ExecuteSqlCommand(string cmdText, params SqlParameter[] paras)  
        { 
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText.ToString(), paras); 
        }
        


        public DataTable ExecutiveSQL(string cmdText, string ConnectionString)// where T : class
        {
            DataTable data = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText.ToString(), null).Tables[0];
            return data;
        }


        public int  ExecuteScalar(string cmdText, string ConnectionString)// where T : class
        {
            int  count = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, cmdText.ToString());
            return count;
        }



        
      

        #region    获取类属性


        public static Dictionary<string, ColumnAttribute> GetAttribute<T>(T model) where T : class
        {
            Type type = typeof(T); 
            return GetAttribute<T>(type);
        }
        /// <summary>
        /// 获取类属性Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<string, ColumnAttribute> GetAttribute<T>(Type type) where T : class
        { 
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

            string tableName = null;
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            if (Classattr!= null)
            {

                tableName = Classattr.TableName;
            }
            else
            {
                foreach (var item in type.GetProperties())
                {
                    tableName = item.Name;

                }
              
            }

            return tableName;


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
        public static string GetPK<T>(Type type, Dictionary<string, ColumnAttribute> Col, object ID) where T : class
        {
       
            string PK = " {0}={1} "; 

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

                        return string.Format(PK, item.Name, ID.ToString());
                    }

                }

            }

            return null;
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        public static string GetPK<T>(T model, Dictionary<string, ColumnAttribute> Col) where T : class
        {
           
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
                        SqlDbType types = new MSSQLCodeFirstDriver().CastDbType(item.PropertyType);
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
            quer.Parsing = new Parsing { DBType = DatabaseType.Sqlserver,ConnectionString= ConnectionString };
            return quer;
        }

     



        #endregion


        T IDrive.FindByID<T>(T model)
        {
            throw new NotImplementedException();
        }


     


           //List<TransactionHelp> test;
           //bool IsSucceed = Test(out test) > 0 && Test(out test) > 0 && Test(out test) > 0; 
           //Transaction(test, IsSucceed); 

        public int TransactionExecuteSqlCommand(ref  List<TransactionHelp> tranlist, string commandText, params SqlParameter[] commandParameters)
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


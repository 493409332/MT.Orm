using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT.Common.Utility;
using System.Data;
using System.Data.SqlClient;
using MT.Orm.DBAttribute;
using System.Diagnostics;
using System.Reflection;
using MT.Orm.CRUDMethod.SqlServer;
using MT.Orm.Sqlserver;
using MT.Common.Utility.Extension;



namespace MT.Orm.CRUDMethod
{
    public  class CRUD:IDrive
    {



        #region Insert
        public int Insert<T>(T model) where T : class
        {
               if (model != null) {
                  
                 //验证并获取列属性
                  Dictionary<string, object> valu=Verification(model,GetAttribute(model));
                  //列名称
                  string ColumnName = AssemblyName(valu);
                  //列的值
                  string values = AssemblyValue(valu);
                  //参数
                  SqlParameter[] paras = AssemblyParameter(valu);
                 //表名
                  String TableName=GetTableName(model);
                   if (valu != null)
                   {
                       try
                       {
                           string cmdText = string.Format(Constant.InnerSql, TableName, ColumnName, values);
                           return SqlHelper.ExecuteNonQuery(SqlEasy.connString, CommandType.Text,cmdText, paras);
                       }
                       catch (Exception e)
                       {
                           
                           throw e;
                       }
                      
                   }
                   }
             return -1;
           }
        #endregion

        #region Delete
        public int Delete<T>(T model) where T : class
        {
            Type type = typeof(T);
            List<SqlParameter> paras = new List<SqlParameter>();
            StringBuilder Where = new StringBuilder();
            Dictionary<string, ColumnAttribute> Col = GetAttribute(model);
            Dictionary<string, object> valu = Verification(model, Col);
            //表名
            String TableName = GetTableName(model);



            foreach (var item in valu.Keys)
            {
                dynamic obj = valu[item.ToString()];
                string v = obj.value;

                if (v != null && v.Length >= 1)
                {
                    Where.AppendFormat(" and {0}=@{0}", item.ToString());

                    paras.Add(new SqlParameter("@" + item.ToString(), v));

                }

            }

            try
            {
                string cmdText = string.Format(Constant.DeleteSql, TableName, Where);
                return SqlHelper.ExecuteNonQuery(SqlEasy.connString, CommandType.Text,cmdText, paras.ToArray());
            }
            catch (Exception)
            {
                return -1;
                throw;
            }
        }
        #endregion

        #region  Update
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
                String TableName = GetTableName(model);


                string cmdText = string.Format(Constant.UptadeSql, TableName, Set, PK);

                try
                {
                    //Console.WriteLine(cmdText.ToString());
                    //Console.Read();
                    return SqlHelper.ExecuteNonQuery(SqlEasy.connString, CommandType.Text, cmdText.ToString(), paras);
                }
                catch (Exception e)
                {

                    throw e;
                }



            }
            return -1;
        }
         #endregion

        #region FindByID
        public List<T> FindByID<T>(T model) where T : class,new()
        {
            if (model != null)
            {
                Type type = typeof(T);

                StringBuilder cmdText = new StringBuilder();
                object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
                TableAttribute Classattr = objs as TableAttribute;

                foreach (PropertyInfo propInfo in type.GetProperties())
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

                                DataTable data = SqlHelper.ExecuteDataset(SqlEasy.connString, CommandType.Text, cmdText.ToString(), paras).Tables[0];


                                if (data.Rows.Count>0)
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
        #endregion

        #region  FindByProperty
      
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
                String TableName = GetTableName(model);



                foreach (var item in valu.Keys)
                {
                    dynamic obj = valu[item.ToString()];
                    string v = obj.value;

                    if (v != null && v.Length >= 1)
                    {

                        Where.AppendFormat(" and {0}=@{0}", item.ToString());

                        paras.Add(new SqlParameter("@" + item.ToString(), v));

                    }

                }
                try
                {
                    string cmdText = string.Format(Constant.FindByProperty, TableName, Where);

                    DataTable data = SqlHelper.ExecuteDataset(SqlEasy.connString, CommandType.Text, cmdText.ToString(), paras.ToArray()).Tables[0];


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
        #endregion

        #region  FindAll
        public List<T> FindAll<T>(T model) where T : class,new()
        {

            Type type = typeof(T);
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("Select * From {0}", Classattr.TableName);
            try
            {
                DataTable data = SqlHelper.ExecuteDataset(SqlEasy.connString, CommandType.Text, cmdText.ToString(), null).Tables[0];
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
        #endregion

        #region  FindLikeProperty
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
                String TableName = GetTableName(model);



                foreach (var item in valu.Keys)
                {
                    dynamic obj = valu[item.ToString()];
                    string v = obj.value;

                    if (v != null && v.Length >= 1)
                    {

                        Where.AppendFormat(" and {0} LIKE '%@{0}%'", item.ToString());

                        paras.Add(new SqlParameter("@" + item.ToString(), v));

                    }

                }
                try
                {
                    string cmdText = string.Format(Constant.FindByProperty, TableName, Where);

                    DataTable data = SqlHelper.ExecuteDataset(SqlEasy.connString, CommandType.Text, cmdText.ToString(), paras.ToArray()).Tables[0];


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
        #endregion

        #region ExecutiveSQL
        public List<T> ExecutiveSQL<T>(string cmdText, SqlParameter[] paras) where T : class, new()
        {
            if (cmdText != null)
            {
                try
                {
                    DataTable data = SqlHelper.ExecuteDataset(SqlEasy.connString, CommandType.Text, cmdText.ToString(), paras).Tables[0];
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
        #endregion

        #region GenerateBuildTable
        public void GenerateBuildTable(List<Type> modeltype)
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
                foreach (var item in type.GetProperties())
                {
                    if (item != null)
                    {
                        //如果此列有Attribute才验证
                        if (Col.ContainsKey(item.Name) && trace.GetFrame(1).GetMethod().ToString() != "System.Collections.Generic.List`1[T] FindByProperty[T](T)")
                        {

                            ColumnAttribute d = Col[item.Name];

                            //非空验证
                            if (d.BeNull)
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
                                    throw new Exception(item.Name.ToString() + "列为自增长列，请不要指定值,并且自增长列的数据类型只能为int");
                                }
                                else if (d.Identity)
                                {
                                    valu.Remove(item.Name.ToString());
                                    continue;
                                }
                            }
                        }
                        //通过验证后
                        SqlDbType types = SqlTypeStringToSqlType(item.PropertyType.FullName.ToString());
                        string values = null;
                        if (item.GetValue(model, new object[0]) != null)
                        {
                            values = item.GetValue(model, new object[0]).ToString();
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

        #region    获取类属性Attribute
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
            foreach (PropertyInfo propInfo in type.GetProperties())
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
        public static string GetTableName<T>(T model) where T : class
        {
            if (model != null)
            {
                Type type = typeof(T);
                object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
                TableAttribute Classattr = objs as TableAttribute;

                return Classattr.TableName;
            }
            else
            {

                return null;
            }

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
                string v = obj.value;
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
                string v = obj.value;


                paras[i] = new SqlParameter("@" + item.ToString(), obj.type);
                paras[i].Value = v;
                i++;

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
            ColumnAttribute iden = Attributes.Values.Where(p => p.Identity).First();

            foreach (var item in valu.Keys)
            {
                dynamic obj = valu[item.ToString()];
                string v = obj.value;
                if (!Attributes[item.ToString()].Identity)
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
                foreach (var item in type.GetProperties())
                {
                    if (item != null)
                    {
                        ColumnAttribute d = Col[item.Name];

                        //主键验证
                        if (d.Primary)
                        {

                            return string.Format(PK, item.Name, item.GetValue(model, new object[0]).ToString());
                        }

                    }

                }
            }
            return null;
        }
        #endregion

        #region SqlDbType
        private static SqlDbType SqlTypeStringToSqlType(string cSharpType)
        {
            SqlDbType ty = new SqlDbType();
            if (cSharpType == null)
            {
                return ty = SqlDbType.VarChar;
            }
            switch (cSharpType)
            {
                case "System.Int32":
                    ty = SqlDbType.Int;

                    break;
                case "String":
                    ty = SqlDbType.VarChar;
                    break;
                case "System.Boolean":
                    ty = SqlDbType.Bit;
                    break;
                case "System.DateTime":
                    ty = SqlDbType.DateTime;
                    break;
                case "System.Decimal":
                    ty = SqlDbType.Decimal;
                    break;
                case "System.Double":
                    ty = SqlDbType.Float;
                    break;
                case "System.Int16":
                    ty = SqlDbType.SmallInt;
                    break;
                case "System.Int64":
                    ty = SqlDbType.BigInt;
                    break;
                case "System.Object":
                    ty = SqlDbType.Binary;
                    break;
                case "System.Single":
                    ty = SqlDbType.Real;
                    break;
                case "System.Guid":
                    ty = SqlDbType.UniqueIdentifier;
                    break;
                default:
                    ty = SqlDbType.VarChar;
                    break;
            }
            return ty;
        }
        #endregion


        public string ConnectionString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsConnectionSuccess()
        {
            throw new NotImplementedException();
        }

        
    }
}

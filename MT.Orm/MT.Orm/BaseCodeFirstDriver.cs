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
using MT.Common.Utility.Config;
using System.Data.OracleClient;
using MT.Orm.Sqlserver;
using MT.Orm.Model;

namespace MT.Orm
{
    public class BaseCodeFirstDriver
    {
        private List<EntityInfoModel> entityColumnList = new List<EntityInfoModel>();

        #region 属性
        public string ConnectionString
        {
            get;
            set;
        }
        public virtual DatabaseType mDatabaseType
        {
            get
            {
                return DatabaseType.Sqlserver;
            }

        }
        public virtual String mtableCommentString
        {
            get
            {
                return "";
            }
        }
        public virtual String mColumnCommentString
        {
            get
            {
                return "";
            }
        }
        public virtual String mRenameTableString
        {
            get
            {
                return "";
            }
        }
        public virtual ArrayList sqlToAddColumnList
        {
            get
            {
                return new ArrayList();
            }
        }
        #endregion

        public virtual ResultType CheckDB(String tableName) { return ResultType.CHECK_FALSE; }
        public virtual void CheckPK(String tableName, List<EntityInfoModel> entityColumnList) { }

        #region 建表
        /// <summary>
        /// 建表
        /// </summary>
        /// <param name="types"></param>
        public virtual void GenerateBuildTable(List<Type> types)
        {
            foreach (var type in types)
            {
                String tableName = String.Empty;
                String tableComment = String.Empty;
                GetTableAttribute(type, ref tableName, ref tableComment);
                String createTableStr = "CREATE TABLE {0} ({1})";

                PropertyInfo[] propInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                ArrayList columnStrList = new ArrayList();
                ArrayList commentList = new ArrayList();
                foreach (var propInfo in propInfos)
                {
                    Dictionary<String, String> columnInfoDic = GetColumnAttribute(tableName, propInfo);
                    columnStrList.Add(columnInfoDic.Keys.FirstOrDefault());
                    commentList.Add(columnInfoDic.Values.FirstOrDefault());
                }
                if (columnStrList.Count > 0)
                {
                    String columnStr = columnStrList.ToArray().Join(",");
                    createTableStr = String.Format(createTableStr, tableName, columnStr);
                    commentList.Add(String.Format(mtableCommentString, Guid.NewGuid(), tableComment, tableName));
                    ArrayList finalSqls = new ArrayList();
                    switch (CheckDB(tableName))
                    {
                        case ResultType.CREATE_DATABASE_SUCCESS:
                        case ResultType.TABLE_NOT_EXIST:
                            finalSqls.Add(createTableStr);
                            finalSqls.AddRange(commentList);
                            break;
                        case ResultType.CHECK_FALSE:
                            break;
                        case ResultType.DELETE_AND_REBUILD:
                            //重建之前备份原数据库
                            finalSqls.Add(String.Format(mRenameTableString, tableName, tableName + "备份" + DateTime.Now.ToLongValue()));
                            finalSqls.Add(createTableStr);
                            break;
                        case ResultType.ONLY_ADD_COLUMNS:
                            finalSqls.AddRange(sqlToAddColumnList);
                            break;
                    };
                    ExcuteSqls(finalSqls);
                    //TODO： 验证主键个数
                    CheckPK(tableName, entityColumnList);
                }
                entityColumnList.Clear(); ;
            }
        }
        #endregion


        #region 取实体的表特性
        /// <summary>
        /// 取实体的表特性
        /// </summary>
        /// <param name="type">实体的type</param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        private static void GetTableAttribute(Type type, ref string tableName, ref string tableComment)
        {
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
        }
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
                    commentStr = String.Format(mColumnCommentString, Guid.NewGuid(), attr.Description, tableName, attr.ColumnName);

                    oneColumnStr = attr.ColumnName + " " + CastToDbType(propInfo.PropertyType, mDatabaseType);
                    String typeName = propInfo.PropertyType.Name.ToString().ToLower();

                    if (typeName.Equals("char") || typeName.Equals("string"))
                    {
                        oneColumnStr += attr.MaxLength.IsNullOrEmpty() ? "(200)" : " (" + attr.MaxLength + ")";
                    }
                    oneColumnStr += (attr.Unique ? " unique" : "")
                        + (mDatabaseType == DatabaseType.Sqlserver ? (attr.Identity ? " identity (1,1) " : "") : "")
                        + (attr.Primary ? "  primary key" : "")
                        + (attr.NotNull ? " not null" : " null");

                    entityColumnList.Add(new EntityInfoModel()
                    {
                        TableName = tableName,
                        ColumnName = attr.ColumnName,
                        IsNullable = attr.NotNull ? "1" : "0",
                        TypeName = CastToDbType(propInfo.PropertyType, mDatabaseType).ToString().ToLower(),
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
                oneColumnStr = propInfo.Name + " " + CastToDbType(propInfo.PropertyType, mDatabaseType);
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
                    TypeName = CastToDbType(propInfo.PropertyType, mDatabaseType).ToString().ToLower(),
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

        #region   C#类型转换成DB类型
        /// <summary>
        /// C#类型转换成DB类型
        /// </summary>
        /// <param name="cSharpType">C#类型</param>
        /// <returns>DB类型</returns>
        public object CastToDbType(Type cSharpType, DatabaseType mDatabaseType)
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
            if (mDatabaseType == DatabaseType.Sqlserver)
            {
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
                return result.ToString();
            }
            else if (mDatabaseType == DatabaseType.Oracle)
            {
                #region C#与Oracle类型
                OracleType result = new OracleType();
                switch (cSharpType.Name.ToString())
                {
                    case "Int32":
                        result = OracleType.Int32;
                        break;
                    case "String":
                        result = OracleType.VarChar;
                        break;
                    case "Boolean":
                        result = OracleType.Byte;
                        break;
                    case "DateTime":
                        result = OracleType.DateTime;
                        break;
                    case "Decimal":
                        result = OracleType.Number;
                        break;
                    case "Double":
                        result = OracleType.Float;
                        break;
                    case "Int16":
                        result = OracleType.Int16;
                        break;
                    //case "Int64":
                    //    result = OracleType.In;
                    //    break;
                    case "Object":
                        result = OracleType.Blob;
                        break;
                    //case "Single":
                    //    result = OracleType.Real;
                    //    break;
                    case "Guid":
                        result = OracleType.Raw;
                        break;
                    case "Char":
                        result = OracleType.Char;
                        break;

                }
                return result.ToString();
                #endregion
            }
            return "";
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

        private void ExcuteSqls(ArrayList finalSqls)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                try
                {
                    SqlHelper.ExecuteSqlTran(finalSqls, ConnectionString, CommandType.Text, transaction);
                }
                catch (Exception e)
                {
                    connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}


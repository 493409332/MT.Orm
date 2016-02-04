 
using MT.Common.Utility.Config;
using MT.Orm.DBAttribute;
using MT.Orm.Sqlserver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MT.Orm
{
    public class DBContext
    {
        string ConnectionStrings = string.Empty;
        string ProviderName = string.Empty;


        public DBContext(string ConnectionString = "MTConnectionString")
        { 
            ConnectionStrings = ConfigHelper.GetConnectionStrings(ConnectionString); 
            ProviderName = ConfigHelper.GetConnectionProviderName(ConnectionString); 
            Init();  
        }

        private void Init()
        {
            PropertyInfo[] ModelProperty = this.GetType().GetProperties().Where(p=>p.PropertyType.GetCustomAttribute(typeof(DBSetAttribute),false)!=null).ToArray() ; 
            IDrive idrive;
            IDatabase idatabase;

            var sqldv= new SqlserverDrive();
            sqldv.ConnectionString = ConnectionStrings;
            if ( ProviderName.StartsWith("SqlServer") )
            {
                idatabase = sqldv;
                idrive = sqldv;
             
                if ( !idrive.IsConnectionSuccess() )
                {
                    throw new Exception("连接字符串验证无效！");
                } 
            }
            else
            {
                idatabase = sqldv;
                idrive = sqldv;
            } 
            List<Type> typeList = new List<Type>();
            foreach ( var item in ModelProperty )
            {
                Type GenericArgument = item.PropertyType.GetGenericArguments().FirstOrDefault();
                //Type DbsetType = typeof(DBSet<>);
                //Type[] typeArgs = { GenericArgument };
                //Type constructed = DbsetType.MakeGenericType(typeArgs); 
                var DbsetInstsnce = Activator.CreateInstance(item.PropertyType);
                PropertyInfo pidrive = item.PropertyType.GetProperty("idrive", BindingFlags.NonPublic | BindingFlags.Instance);
                pidrive.SetValue(DbsetInstsnce, idrive, null);
                item.SetValue(this, DbsetInstsnce, null);
                typeList.Add(GenericArgument);
            }
           // idrive.GenerateBuildTable(typeList);

            PropertyInfo DatabaseProperty = this.GetType().GetProperty("Database");
            DatabaseProperty.SetValue(this, idatabase, null);
        } 
 

        public DBSet<T> Set<T>() where T : class,new()
        {
            PropertyInfo[] ModelProperty = this.GetType().GetProperties();
            Type DbsetType = typeof(DBSet<>);
            Type[] typeArgs = { typeof(T) };
            Type constructed = DbsetType.MakeGenericType(typeArgs);
            
            PropertyInfo DBSetPropertyInfo = ModelProperty.Where( p => p.PropertyType==constructed  ).FirstOrDefault(); 
            if ( DBSetPropertyInfo != null )
            {
                var DBSetval = DBSetPropertyInfo.GetValue(this);
                if ( DBSetval is DBSet<T> )
                {
                    return DBSetval as DBSet<T>;
                } 
            }
            return null;
        }
        public IDatabase Database { get; set; }

        public void Transaction(List<TransactionHelp> tranlist, bool IsSucceed)
        {
            if ( IsSucceed )
            {
                tranlist.Where(p => p.Commit != null).Select(p => p.Commit).All((p) =>
                {
                    p();
                    return true;
                });
            }
            else
            {
                tranlist.Where(p => p.Rollback != null).Select(p => p.Rollback).All((p) =>
                {
                    p();
                    return true;
                });
            }

            DisposeSqlConn(tranlist.Select(p => p.DispostSL).ToArray());

        }

        public void DisposeSqlConn(object[] conn)
        {
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Reset();
            time.Start();
            try
            {
                conn.All((p) => { if ( p is SqlConnection ) { ( (SqlConnection) p ).Dispose(); } else { dynamic pp = p; pp.Dispose(); } return true; });

            }
            catch ( Exception )
            {
                conn.All((p) =>
                {
                    if ( p is SqlConnection )
                    {
                        ( (SqlConnection) p ).Dispose();
                    }
                    else
                    {
                        dynamic pp = p;
                        //  pp.Close();
                        pp.Dispose();
                    }

                    return true;
                });
            } time.Stop();
            int aaaa = time.Elapsed.Milliseconds;
        }



    }

 
    //public class EntitytoData : DBContext
    //{
    //    public EntitytoData()
    //        : base("SQLServerContext")
    //    { 

    //    }
    //    public DBSet<EntityTest> EntityTest { get; set; }

    
    //}


}

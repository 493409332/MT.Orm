using MT.Common.Utility.Extension;
using MT.Orm.DBAttribute;
using MT.Orm.Sqlserver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace MT.Orm
{
    #region  接口
    public interface IQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface IWhereQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface IOrderByQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface ISelectQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface IGroupByQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface ISkipQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface ICountQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface ITakeQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    public interface IFirstOrDefaultQUeryable<out T>
    {
        Parsing Parsing { get; set; }
    }

    #endregion

    #region 类
    public class Parsing
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DBType { get; set; }
        /// <summary>
        /// where解析树
        /// </summary>
        public System.Linq.Expressions.Expression WhereExpression { get; set; }

        public System.Linq.Expressions.Expression OrderExpression { get; set; }
        public Dictionary<string, bool> OrderByName { get; set; }
        // public List<string> OrderByDescendingName { get; set; }
        public Dictionary<string, string> selectDictionary { get; set; }

        public int TopCount { get; set; }

        public string Count { get; set; }

        public int Takecount { get; set; }

        public string SqlCommands { get; set; }

        public List<GroupByItem> GroupByList { get; set; }

        public string PK_Name { get; set; }

        public string TableName { get; set; }

        public string ConnectionString { get; set; }

        public ConstructorInfo constructor { get; set; }

    }

    public class GroupByItem
    {
        public string NewName { get; set; }

        public string OldName { get; set; }

        public string DBFun { get; set; }
    }

    public static class DBFuc
    {
        #region GroupBy
        /// <summary>
        /// 仅限 ORM映射GroupBy使用
        /// </summary> 
        public static  int? DBCount(this object value)
        {
            return null;
        }
        /// <summary>
        /// 仅限 ORM映射GroupBy使用
        /// </summary> 
        public static int? DBMax(this object value)
        {
            return null;
        }
        /// <summary>
        /// 仅限 ORM映射GroupBy使用
        /// </summary> 
        public static int? DBMin(this object value)
        {
            return null;
        }
        /// <summary>
        /// 仅限 ORM映射GroupBy使用
        /// </summary> 
        public static int? DBSum(this object value)
        {
            return null;
        }
        /// <summary>
        /// 仅限 ORM映射GroupBy使用
        /// </summary> 
        public static object DBAvg(this object value)
        {

            return null;
        }

        #endregion

        # region ContainsMt
      
        /// <summary>
        /// 仅限ORM映射ContainsMt使用
        /// </summary> 
        public static bool ContainsMt<T>(this IEnumerable<T> list, T item)
        {
            return true;
        }
        /// <summary>
        /// 仅限ORM映射ContainsMt使用
        /// </summary> 
        public static bool ContainsMt<T>(this List<T> list, T item)
        {
            return true;
        }
        /// <summary>
        /// 仅限ORM映射ContainsMt使用
        /// </summary> 
        public static bool ContainsMt<T>(this IList<T> list, T item)
        {
            return true;
        }
        /// <summary>
        /// 仅限ORM映射ContainsMt使用
        /// </summary> 
        public static bool ContainsMt<T>(this ICollection<T> list, T item)
        {
            return true;
        }

        /// <summary>
        /// 仅限ORM映射ContainsMt使用
        /// </summary> 
        public static bool ContainsMt<T>(this T[] list, T item)
        {
            //ew List<string>().MtContains("");
            // List<int> aaa = new List<int>();
            // aaa.MtContains(5);
            return true;
        }
     
        #endregion

    }

    public class RBaseQUeryable<T> : IQUeryable<T>, IWhereQUeryable<T>, IOrderByQUeryable<T>
        , ISelectQUeryable<T>, IGroupByQUeryable<T>, ISkipQUeryable<T>, ITakeQUeryable<T>, ICountQUeryable<T>, IFirstOrDefaultQUeryable<T>
    {
        public Parsing Parsing
        { get; set; }
    }
    //    return (RBaseQUeryable<TSource>) source;
    #endregion


    public static class QUeryable
    {
        public static IQUeryable<TSource> ToIQUeryable<TSource>(this IWhereQUeryable<TSource> source)
        {
            return (RBaseQUeryable<TSource>)source;
        }

        #region  where
        #region Where  OrderBy  Skip  Take   Select  END
        /// <summary>
        /// 条件语句
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IWhereQUeryable<TSource> Where<TSource>(this IQUeryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {

            return _Where((RBaseQUeryable<TSource>)source, predicate);
        }


        /// <summary>
        /// 条件语句
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IWhereQUeryable<TSource> Where<TSource>(this IWhereQUeryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return _Where((RBaseQUeryable<TSource>)source, predicate);
            #region
            //if (source == null)
            //{
            //    throw new Exception("source");
            //}
            //if (predicate == null)
            //{
            //    throw new Exception("predicate");
            //}


            //if (predicate.Body is BinaryExpression)
            //{
            //    BinaryExpression Rbinary = ((BinaryExpression)predicate.Body);


            //    if (source.Parsing.WhereExpression is BinaryExpression)
            //    {
            //        BinaryExpression Lbinary = ((BinaryExpression)source.Parsing.WhereExpression);
            //        source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
            //    }
            //    else if (source.Parsing.WhereExpression is MethodCallExpression)
            //    {
            //        MethodCallExpression Lbinary = ((MethodCallExpression)source.Parsing.WhereExpression);
            //        source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
            //    }
            //    else
            //    {
            //        source.Parsing.WhereExpression = Rbinary;
            //    }

            //    return (RBaseQUeryable<TSource>)source;
            //}
            //else if (predicate.Body is MethodCallExpression)
            //{
            //    MethodCallExpression Rbinary = ((MethodCallExpression)predicate.Body);


            //    if (source.Parsing.WhereExpression is BinaryExpression)
            //    {
            //        BinaryExpression Lbinary = ((BinaryExpression)source.Parsing.WhereExpression);
            //        source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
            //    }
            //    else if (source.Parsing.WhereExpression is MethodCallExpression)
            //    {
            //        MethodCallExpression Lbinary = ((MethodCallExpression)source.Parsing.WhereExpression);
            //        source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
            //    }
            //    else
            //    {
            //        source.Parsing.WhereExpression = Rbinary;
            //    }
            //    return (RBaseQUeryable<TSource>)source;
            //}
            //else
            //{
            //    throw new Exception("No BinaryExpression");
            //}
            #endregion
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <param name="isdec"></param>
        /// <returns></returns>
        public static IOrderByQUeryable<TSource> OrderBy<TSource, TKey>(this IWhereQUeryable<TSource> source, Expression<Func<TSource, TKey>> order, bool isdec = true)
        {
            string orderstr = string.Empty;

            if (order.Body is UnaryExpression)
            {
                orderstr = ((MemberExpression)((UnaryExpression)order.Body).Operand).Member.Name;
            }
            else if (order.Body is MemberExpression)
            {
                orderstr = ((MemberExpression)order.Body).Member.Name;
            }
            else if (order.Body is ParameterExpression)
            {
                orderstr = ((ParameterExpression)order.Body).Type.Name;
            }
            else if (order.Body is NewExpression)
            {
                orderstr = ((NewExpression)order.Body).Members.First().Name;


            }
            /*******************************************************************/

            if (isdec)
            {
                // List<string> strlist = new List<string>();
                Dictionary<string, bool> strlist = new Dictionary<string, bool>();
                if (source.Parsing.OrderByName != null)
                {
                    if (source.Parsing.OrderByName.Count > 0)
                    {
                        strlist = source.Parsing.OrderByName;
                    }
                }
                strlist.Add(orderstr, true);
                source.Parsing.OrderByName = strlist;
            }
            //else
            //{
            //    Dictionary<string, bool> strlist = new Dictionary<string, bool>();
            //    //List<string> strlist = new List<string>();
            //    //if (source.Parsing.OrderByDescendingName != null)
            //    {
            //        strlist = source.Parsing.OrderByDescendingName;
            //    }
            //    strlist.Add(orderstr);
            //    source.Parsing.OrderByDescendingName = strlist;
            //}

            return (RBaseQUeryable<TSource>)source;

        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <param name="isdec"></param>
        /// <returns></returns>
        public static IOrderByQUeryable<TSource> OrderBy<TSource, TKey>(this IOrderByQUeryable<TSource> source, Expression<Func<TSource, TKey>> order, bool isdec = true)
        {
            string orderstr = string.Empty;

            if (order.Body is UnaryExpression)
            {
                orderstr = ((MemberExpression)((UnaryExpression)order.Body).Operand).Member.Name;
            }
            else if (order.Body is MemberExpression)
            {
                orderstr = ((MemberExpression)order.Body).Member.Name;
            }
            else if (order.Body is ParameterExpression)
            {
                orderstr = ((ParameterExpression)order.Body).Type.Name;
            }

            if (isdec)
            {
                // List<string> strlist = new List<string>();
                Dictionary<string, bool> strlist = new Dictionary<string, bool>();
                if (source.Parsing.OrderByName != null)
                {
                    strlist = source.Parsing.OrderByName;
                }
                strlist.Add(orderstr, true);
                source.Parsing.OrderByName = strlist;
            }
            //else
            //{
            //    List<string> strlist = new List<string>();
            //    if (source.Parsing.OrderByDescendingName != null)
            //    {
            //        strlist = source.Parsing.OrderByDescendingName;
            //    }
            //    strlist.Add(orderstr);
            //    source.Parsing.OrderByDescendingName = strlist;
            //}

            return (RBaseQUeryable<TSource>)source;

        }
        /// <summary>
        /// 排除多少条数据  跳过   skip(1)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ISkipQUeryable<TSource> Skip<TSource>(this IOrderByQUeryable<TSource> source, int count)
        {
            return _Skip((RBaseQUeryable<TSource>)source,count);
           
        }
        /// <summary>
        /// 取  取多少条数据 Take(1)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ITakeQUeryable<TSource> Take<TSource>(this ISkipQUeryable<TSource> source, int count)
        {

            return _Take((RBaseQUeryable<TSource>)source, count);
        }

        public static ISelectQUeryable<TResult> Select<TSource, TResult>(this ITakeQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return null;
            //return _Select((RBaseQUeryable<TResult>)source, selector);
            #region
            //ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();
            //newsource.Parsing = source.Parsing;

            //Type type = typeof(TSource);
            //object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            //TableAttribute Classattr = objs as TableAttribute;
            //string tableName = Classattr.TableName;


            //var PKColumnquer = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
            //{
            //    var attr = p.GetCustomAttributes(typeof(ColumnAttribute), false).Where(PP =>
            //((ColumnAttribute)PP).Primary

            //);
            //    if (attr.Count() > 0)
            //        return true;
            //    else if (new string[] { "ID", "id", "Id", "iD" }.Contains(p.Name))
            //        return true;
            //    return false;

            //}).ToList();
            //source.Parsing.PK_Name = PKColumnquer.First().Name.ToString();

            //string selectorstr = string.Empty;
            //Dictionary<string, string> selectDictionary = new Dictionary<string, string>();
            //if (selector.Body is NewExpression)
            //{
            //    var newexpression = (NewExpression)selector.Body;
            //    ConstructorInfo newConstructor = typeof(TSource).GetConstructor(new Type[] { });
            //    newsource.Parsing.constructor = newConstructor;
            //    if (typeof(TResult) == newConstructor.GetType())
            //    {

            //    }

            //    var selectArguments = newexpression.Arguments;


            //    var selectMembers = newexpression.Members;
            //    for (int i = 0; i < selectArguments.Count; i++)
            //    {
            //        string selectArgumentstr = string.Empty;
            //        if (selectArguments[i] is UnaryExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
            //        }
            //        else if (selectArguments[i] is MemberExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
            //        }
            //        else if (selectArguments[i] is ParameterExpression)
            //        {
            //            selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
            //        }

            //        selectDictionary.Add(selectArgumentstr, selectMembers[i].Name);
            //    }

            //}
            //else if (selector.Body is MemberExpression)
            //{

            //    string Name = ((MemberExpression)selector.Body).Member.Name;
            //    selectDictionary.Add(Name, Name);
            //}


            //newsource.Parsing.TableName = tableName;
            //newsource.Parsing.selectDictionary = selectDictionary;

            //return newsource;
            #endregion
        }
        /// <summary>
        /// 分组
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IGroupByQUeryable<TResult> GroupBy<TSource, TResult>(this ITakeQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {

            IGroupByQUeryable<TResult> newsource = new RBaseQUeryable<TResult>();
            newsource.Parsing = source.Parsing;

            if (selector.Body is NewExpression)
            {
                var selectArguments = ((NewExpression)selector.Body).Arguments;
                var selectMembers = ((NewExpression)selector.Body).Members;
                var grouuplist = new List<GroupByItem>();

                for (int i = 0; i < selectArguments.Count; i++)
                {
                    string selectArgumentstr = null;

                    GroupByItem groupmodel = new GroupByItem();

                    string Funstr = string.Empty;
                    if (selectArguments[i] is UnaryExpression)
                    {
                        selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
                    }
                    else if (selectArguments[i] is MemberExpression)
                    {
                        selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
                    }
                    else if (selectArguments[i] is ParameterExpression)
                    {
                        selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
                    }
                    else if (selectArguments[i] is MethodCallExpression)
                    {
                        MethodCallExpression methodcall = selectArguments[i] as MethodCallExpression;

                        if (methodcall.Arguments[0] is UnaryExpression)
                        {
                            UnaryExpression unary = methodcall.Arguments[0] as UnaryExpression;
                            if (unary.Operand is MemberExpression)
                            {
                                selectArgumentstr = ((MemberExpression)unary.Operand).Member.Name;
                            }
                        }
                        if (methodcall.Method == typeof(DBFuc).GetMethod("DBCount"))
                        {
                            Funstr = "count({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBMax"))
                        {
                            Funstr = "max({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBMin"))
                        {
                            Funstr = "min({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBSum"))
                        {
                            Funstr = "sum({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBavg"))
                        {
                            Funstr = "avg({0})";
                        }
                    }
                    groupmodel.NewName = selectMembers[i].Name;
                    groupmodel.OldName = selectArgumentstr;
                    groupmodel.DBFun = Funstr;
                    grouuplist.Add(groupmodel);


                }
                newsource.Parsing.GroupByList = grouuplist;


            }
            else if (true)
            {

            }
            else
            {

            }



            return newsource;
        }
        #endregion

        #region Where  OrderBy  Take  Select End
        public static ITakeQUeryable<TSource> Take<TSource>(this IOrderByQUeryable<TSource> source, int count)
        {
           
            return _Take((RBaseQUeryable<TSource>)source,count);
        }
        #endregion

        #region    Where  OrderBy  Skip  Select
        public static ISelectQUeryable<TResult> Select<TSource, TResult>(this ISkipQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return null;

           // return _Select((RBaseQUeryable<TResult>)source, selector);
            #region
            //ISelectQUeryable<TResult> newsource = new RBaseQUeryable<TResult>();
            //newsource.Parsing = source.Parsing;
            //string selectorstr = string.Empty;
            //Dictionary<string, string> selectDictionary = new Dictionary<string, string>();
            //Type type = typeof(TSource);
            //object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            //TableAttribute Classattr = objs as TableAttribute;
            //string tableName = Classattr.TableName;

            //ConstructorInfo newConstructor = typeof(TSource).GetConstructor(new Type[] { });
            //newsource.Parsing.constructor = newConstructor;


            //if (selector.Body is NewExpression)
            //{
            //    var selectArguments = ((NewExpression)selector.Body).Arguments;
            //    var selectMembers = ((NewExpression)selector.Body).Members;
            //    for (int i = 0; i < selectArguments.Count; i++)
            //    {
            //        string selectArgumentstr = string.Empty;
            //        if (selectArguments[i] is UnaryExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
            //        }
            //        else if (selectArguments[i] is MemberExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
            //        }
            //        else if (selectArguments[i] is ParameterExpression)
            //        {
            //            selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
            //        }

            //        selectDictionary.Add(selectArgumentstr, selectMembers[i].Name);
            //    }

            //}
            //else if (selector.Body is MemberExpression)
            //{
            //    string Name = ((MemberExpression)selector.Body).Member.Name;
            //    selectDictionary.Add(Name, Name);
            //}
            //else
            //{
            //    throw new Exception("No NewExpression");
            //}
            //newsource.Parsing.selectDictionary = selectDictionary;
            //newsource.Parsing.TableName = tableName;
            //return newsource;
            #endregion
        }
        #endregion

        #region  Where  OrderBy  Select
        public static ISelectQUeryable<TResult> Select<TSource, TResult>(this IOrderByQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return null;
            //return _Select((RBaseQUeryable<TResult>)source, selector);
            #region
            //ISelectQUeryable<TResult> newsource = new RBaseQUeryable<TResult>();

            //Type type = typeof(TSource);
            //object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            //TableAttribute Classattr = objs as TableAttribute;
            //string tableName = Classattr.TableName;
            //newsource.Parsing = source.Parsing;
            //string selectorstr = string.Empty;

            //ConstructorInfo newConstructor = typeof(TSource).GetConstructor(new Type[] { });
            //newsource.Parsing.constructor = newConstructor;


            //Dictionary<string, string> selectDictionary = new Dictionary<string, string>();

            //if (selector.Body is NewExpression)
            //{
            //    var newexpression = (NewExpression)selector.Body;




            //    var selectArguments = ((NewExpression)selector.Body).Arguments;
            //    var selectMembers = ((NewExpression)selector.Body).Members;
            //    for (int i = 0; i < selectArguments.Count; i++)
            //    {
            //        string selectArgumentstr = string.Empty;
            //        if (selectArguments[i] is UnaryExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
            //        }
            //        else if (selectArguments[i] is MemberExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
            //        }
            //        else if (selectArguments[i] is ParameterExpression)
            //        {
            //            selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
            //        }

            //        selectDictionary.Add(selectArgumentstr, selectMembers[i].Name);
            //    }


            //}
            //else if (selector.Body is MemberExpression)
            //{
            //    string Name = ((MemberExpression)selector.Body).Member.Name;
            //    selectDictionary.Add(Name, Name);
            //}
            //else
            //{
            //    throw new Exception("No NewExpression");
            //}


            //newsource.Parsing.selectDictionary = selectDictionary;
            //newsource.Parsing.TableName = tableName;


            //return newsource;
            #endregion
        }

        #endregion

        #region Where  Skip  Take Select
        public static ISkipQUeryable<TSource> Skip<TSource>(this IWhereQUeryable<TSource> source, int count)
        {

            if (source == null)
            {
                throw new Exception("source");
            }
            if (count < 0)
            {
                throw new Exception("Skipcount");
            }
            Type type = typeof(TSource);
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            string tableName = Classattr.TableName;
            source.Parsing.TableName = tableName;
            var PKColumnquer = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
            {
                var attr = p.GetCustomAttributes(typeof(ColumnAttribute), false).Where(PP =>
            ((ColumnAttribute)PP).Primary

            );
                if (attr.Count() > 0)
                    return true;
                else if (new string[] { "ID", "id", "Id", "iD" }.Contains(p.Name))
                    return true;
                return false;

            }).ToList();
            source.Parsing.PK_Name = PKColumnquer.First().Name.ToString();
            source.Parsing.TopCount = count;
            return (RBaseQUeryable<TSource>)source;
        }
        #endregion

        #region Where  Take  Select
        public static ITakeQUeryable<TSource> Take<TSource>(this IWhereQUeryable<TSource> source, int count)
        {
          return  _Take((RBaseQUeryable<TSource>)source,count);
        }
        #endregion


        #region Where  Select
        public static ISelectQUeryable<TResult> Select<TSource, TResult>(this IWhereQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return null;

            //return _Select((RBaseQUeryable<TResult>)source, selector);
            #region
            //ISelectQUeryable<TResult> newsource = new RBaseQUeryable<TResult>();
            //string tableName = GetTabName(typeof(TSource));
            //string selectorstr = string.Empty;
            //Dictionary<string, string> selectDictionary = new Dictionary<string, string>();

            //ConstructorInfo newConstructor = typeof(TSource).GetConstructor(new Type[] { });
            //newsource.Parsing.constructor = newConstructor;

            //if (selector.Body is NewExpression)
            //{

            //    var selectArguments = ((NewExpression)selector.Body).Arguments;
            //    var selectMembers = ((NewExpression)selector.Body).Members;
            //    for (int i = 0; i < selectArguments.Count; i++)
            //    {
            //        string selectArgumentstr = string.Empty;
            //        if (selectArguments[i] is UnaryExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
            //        }
            //        else if (selectArguments[i] is MemberExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
            //        }
            //        else if (selectArguments[i] is ParameterExpression)
            //        {
            //            selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
            //        }

            //        selectDictionary.Add(selectArgumentstr, selectMembers[i].Name);
            //    }

            //}
            //else if (selector.Body is MemberExpression)
            //{
            //    string Name = ((MemberExpression)selector.Body).Member.Name;
            //    selectDictionary.Add(Name, Name);
            //}
            //else
            //{
            //    throw new Exception("No NewExpression");
            //}


            //newsource.Parsing.selectDictionary = selectDictionary;
            //newsource.Parsing.TableName = tableName;

            //return newsource;
            #endregion
        }
        #endregion

        public static IWhereQUeryable<TSource> Where<TSource>(this IGroupByQUeryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {

            return _Where((RBaseQUeryable<TSource>)source, predicate);
        }
        #endregion

        #region _where
        public static IWhereQUeryable<TSource> _Where<TSource>(this RBaseQUeryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {

            if (source == null)
            {
                throw new Exception("source");
            }
            if (predicate == null)
            {
                throw new Exception("predicate");
            }


            if (predicate.Body is BinaryExpression)
            {
                BinaryExpression Rbinary = ((BinaryExpression)predicate.Body);


                if (source.Parsing.WhereExpression is BinaryExpression)
                {
                    BinaryExpression Lbinary = ((BinaryExpression)source.Parsing.WhereExpression);
                    source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
                }
                else if (source.Parsing.WhereExpression is MethodCallExpression)
                {
                    MethodCallExpression Lbinary = ((MethodCallExpression)source.Parsing.WhereExpression);
                    source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
                }
                else
                {
                    source.Parsing.WhereExpression = Rbinary;
                }

                return (RBaseQUeryable<TSource>)source;
            }
            else if (predicate.Body is MethodCallExpression)
            {
                MethodCallExpression Rbinary = ((MethodCallExpression)predicate.Body);


                if (source.Parsing.WhereExpression is BinaryExpression)
                {
                    BinaryExpression Lbinary = ((BinaryExpression)source.Parsing.WhereExpression);
                    source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
                }
                else if (source.Parsing.WhereExpression is MethodCallExpression)
                {
                    MethodCallExpression Lbinary = ((MethodCallExpression)source.Parsing.WhereExpression);
                    source.Parsing.WhereExpression = Expression.AndAlso(Rbinary, Lbinary);
                }
                else
                {
                    source.Parsing.WhereExpression = Rbinary;
                }
                return (RBaseQUeryable<TSource>)source;
            }
            else
            {
                throw new Exception("No BinaryExpression");
            }
        }
        #endregion


        #region _Select
        public static ISelectQUeryable<TResult> _Select<TSource, TResult>(RBaseQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            ISelectQUeryable<TResult> newsource = new RBaseQUeryable<TResult>();
            newsource.Parsing = source.Parsing;

            //Type type = typeof(TSource);
            //object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            //TableAttribute Classattr = objs as TableAttribute;
           // string tableName = Classattr.TableName;
            string tableName = GetTabName(typeof(TSource));

            var PKColumnquer = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
            {
                var attr = p.GetCustomAttributes(typeof(ColumnAttribute), false).Where(PP =>
            ((ColumnAttribute)PP).Primary

            );
                if (attr.Count() > 0)
                    return true;
                else if (new string[] { "ID", "id", "Id", "iD" }.Contains(p.Name))
                    return true;
                return false;

            }).ToList();
            source.Parsing.PK_Name = PKColumnquer.First().Name.ToString();

            string selectorstr = string.Empty;
            Dictionary<string, string> selectDictionary = new Dictionary<string, string>();
            if (selector.Body is NewExpression)
            {
                var newexpression = (NewExpression)selector.Body;

                 
                newsource.Parsing.constructor = newexpression.Constructor;
            //    ConstructorInfo t = typeof(TResult).GetConstructor(new Type[] { });

                var selectArguments = newexpression.Arguments;


                var selectMembers = newexpression.Members;
                for (int i = 0; i < selectArguments.Count; i++)
                {
                    string selectArgumentstr = string.Empty;
                    if (selectArguments[i] is UnaryExpression)
                    {
                        selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
                    }
                    else if (selectArguments[i] is MemberExpression)
                    {
                        selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
                    }
                    else if (selectArguments[i] is ParameterExpression)
                    {
                        selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
                    }

                    selectDictionary.Add(selectArgumentstr, selectMembers[i].Name);
                }

            }
            else if (selector.Body is MemberExpression)
            {
                var quer = ((MemberExpression)selector.Body).Member;
                string Name = quer.Name;
                selectDictionary.Add(Name, Name);
                
                //   newsource.Parsing.constructor = quer.DeclaringType.GetConstructor(new Type[]{});

            }


            newsource.Parsing.TableName = tableName;
            newsource.Parsing.selectDictionary = selectDictionary;

            return (RBaseQUeryable<TResult>)newsource;
        }
        #endregion

        #region  Skip

        /// <summary>
        /// 排除 ，跳过结果中的X条数据   Skip(1)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ISkipQUeryable<TSource> Skip<TSource>(this IQUeryable<TSource> source, int count)
        {
            return _Skip((RBaseQUeryable<TSource>)source,count);
         
        }
        #endregion

        #region Take
        /// <summary>
        /// Take(1)   取的结果中的X条数据
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ITakeQUeryable<TSource> Take<TSource>(this IQUeryable<TSource> source, int count)
        {

            return _Take((RBaseQUeryable < TSource >)source, count);
        }
        
         private static ITakeQUeryable<TSource> _Take<TSource>( RBaseQUeryable <TSource> source, int count)
        {
            if (source == null)
            {
                throw new Exception("source");
            }
            if (count < 0)
            {
                throw new Exception("Takecount");
            }
            source.Parsing.Takecount = count;

            return (RBaseQUeryable<TSource>)source;
        }

         private static ISkipQUeryable<TSource> _Skip<TSource>(RBaseQUeryable<TSource> source, int count)
         {
             if (source == null)
             {
                 throw new Exception("source");
             }
             if (count < 0)
             {
                 throw new Exception("Skipcount");
             }
             source.Parsing.TopCount = count;
             return (RBaseQUeryable<TSource>)source;
         }

        

        #endregion

        #region OrderBy
        /// <summary>
        /// 分组
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <param name="isdec"></param>
        /// <returns></returns>
        public static IOrderByQUeryable<TSource> OrderBy<TSource, TKey>(this IQUeryable<TSource> source, Expression<Func<TSource, TKey>> order, bool isdec = true)
        {
            string orderstr = string.Empty;

            if (order.Body is UnaryExpression)
            {
                orderstr = ((MemberExpression)((UnaryExpression)order.Body).Operand).Member.Name;
            }
            else if (order.Body is MemberExpression)
            {
                orderstr = ((MemberExpression)order.Body).Member.Name;
            }
            else if (order.Body is ParameterExpression)
            {
                orderstr = ((ParameterExpression)order.Body).Type.Name;
            }

            if (isdec)
            {
                Dictionary<string, bool> strlist = new Dictionary<string, bool>();
                if (source.Parsing.OrderByName != null)
                {
                    strlist = source.Parsing.OrderByName;
                }
                strlist.Add(orderstr, true);
                source.Parsing.OrderByName = strlist;
            }
            //else
            //{
            //    List<string> strlist = new List<string>();
            //    if ( source.Parsing.OrderByDescendingName != null )
            //    {
            //        strlist = source.Parsing.OrderByDescendingName;
            //    }
            //    strlist.Add(orderstr);
            //    source.Parsing.OrderByDescendingName = strlist;
            //}

            return (RBaseQUeryable<TSource>)source;

        }
        #endregion

        #region Select
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ISelectQUeryable<TResult> Select<TSource, TResult>(this IQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return _Select<TSource, TResult>((RBaseQUeryable<TSource>)source, selector);
            #region
            //Type type = typeof(TSource);
            //object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            //TableAttribute Classattr = objs as TableAttribute;

            //string tableName = Classattr.TableName;

            //var PKColumnquer = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
            //{
            //    var attr = p.GetCustomAttributes(typeof(ColumnAttribute), false).Where(PP =>
            //((ColumnAttribute)PP).Primary

            //);
            //    if (attr.Count() > 0)
            //        return true;
            //    else if (new string[] { "ID", "id", "Id", "iD" }.Contains(p.Name))
            //        return true;
            //    return false;

            //}).ToList();
            //ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();
            //newsource.Parsing = source.Parsing;
            //ConstructorInfo newConstructor = typeof(TSource).GetConstructor(new Type[] { });
            //newsource.Parsing.constructor = newConstructor;
            //string selectorstr = string.Empty;
            //Dictionary<string, string> selectDictionary = new Dictionary<string, string>();
            //if (selector.Body is NewExpression)
            //{
            //    var selectArguments = ((NewExpression)selector.Body).Arguments;
            //    var selectMembers = ((NewExpression)selector.Body).Members;
            //    for (int i = 0; i < selectArguments.Count; i++)
            //    {
            //        string selectArgumentstr = string.Empty;
            //        if (selectArguments[i] is UnaryExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
            //        }
            //        else if (selectArguments[i] is MemberExpression)
            //        {
            //            selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
            //        }
            //        else if (selectArguments[i] is ParameterExpression)
            //        {
            //            selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
            //        }

            //        selectDictionary.Add(selectArgumentstr, selectMembers[i].Name);
            //    }
            //}
            //else if (selector.Body is MemberExpression)
            //{
            //    string Name = ((MemberExpression)selector.Body).Member.Name;
            //    selectDictionary.Add(Name, Name);
            //}
            //else
            //{
            //    throw new Exception("No NewExpression");
            //}
            //newsource.Parsing.selectDictionary = selectDictionary;
            //newsource.Parsing.TableName = tableName;
            //return newsource;
            #endregion
        }


        public static ISelectQUeryable<TResult> Select<TSource, TResult>(this IGroupByQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return null;
           // return _Select((RBaseQUeryable<TResult>)source, selector);
        }
        #endregion

        #region GroupBy
        public static IGroupByQUeryable<TResult> GroupBy<TSource, TResult>(this IQUeryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            IGroupByQUeryable<TResult> newsource = new RBaseQUeryable<TResult>();
            newsource.Parsing = source.Parsing;
            GroupByItem groupmodel = new GroupByItem();
            var grouuplist = new List<GroupByItem>();
            if (selector.Body is NewExpression)
            {
                var selectArguments = ((NewExpression)selector.Body).Arguments;
                var selectMembers = ((NewExpression)selector.Body).Members;
               

                for (int i = 0; i < selectArguments.Count; i++)
                {
                    string selectArgumentstr = null;

                  

                    string Funstr = string.Empty;
                    if (selectArguments[i] is UnaryExpression)
                    {
                        selectArgumentstr = ((MemberExpression)((UnaryExpression)selectArguments[i]).Operand).Member.Name;
                    }
                    else if (selectArguments[i] is MemberExpression)
                    {
                        selectArgumentstr = ((MemberExpression)selectArguments[i]).Member.Name;
                    }
                    else if (selectArguments[i] is ParameterExpression)
                    {
                        selectArgumentstr = ((ParameterExpression)selectArguments[i]).Type.Name;
                    }
                  
                    else if (selectArguments[i] is MethodCallExpression)
                    {
                        MethodCallExpression methodcall = selectArguments[i] as MethodCallExpression;

                        if (methodcall.Arguments[0] is UnaryExpression)
                        {
                            UnaryExpression unary = methodcall.Arguments[0] as UnaryExpression;
                            if (unary.Operand is MemberExpression)
                            {
                                selectArgumentstr = ((MemberExpression)unary.Operand).Member.Name;
                            }
                        }
                        if (methodcall.Method == typeof(DBFuc).GetMethod("DBCount"))
                        {
                            Funstr = "count({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBMax"))
                        {
                            Funstr = "max({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBMin"))
                        {
                            Funstr = "min({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBSum"))
                        {
                            Funstr = "SUM({0})";
                        }
                        else if (methodcall.Method == typeof(DBFuc).GetMethod("DBavg"))
                        {
                            Funstr = "avg({0})";
                        }
                    }
                    groupmodel.NewName = selectMembers[i].Name;
                    groupmodel.OldName = selectArgumentstr;
                    groupmodel.DBFun = Funstr;
                    grouuplist.Add(groupmodel);
                }
                newsource.Parsing.GroupByList = grouuplist;


            } else if (selector.Body is MemberExpression)   
                {
                    var NewName = ((MemberExpression)selector.Body).Member.Name;
                    if (NewName != null)
                    {
                        groupmodel.NewName = NewName;
                    }
                    grouuplist.Add(groupmodel);
                }



            return newsource;
        }
        #endregion

        #region TOList

        #region Where=》 Tolist
        public static List<TSource> ToList<TSource>(this IWhereQUeryable<TSource> source)
        {
            string wherestr = null, TableName = null, Sql = null;
            ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();
            newsource.Parsing = source.Parsing;
            TableName = GetTabName(typeof(TSource));

            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });

            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);
            }



            Sql = " select {0} from  {1}  where {2} ";

            Sql = string.Format(Sql, " * ", TableName, wherestr);

            SqlserverDrive driver = new SqlserverDrive();

            return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor);
        }
        #endregion

        #region GroupBy=》 Tolist
        public static List<TSource> ToList<TSource>(this IGroupByQUeryable<TSource> source)
        {
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            string sql = "SELECT {0} {1} FROM {2}  {3}";
            string select = null;
            string TableName = GetTabName(typeof(TSource));
            string DBFun = null;
            if (source.Parsing.GroupByList!=null&&source.Parsing.GroupByList.Count > 0)
            {
                int i = 0;
                foreach (var item in source.Parsing.GroupByList)
                {
                    if (item.DBFun != null)
                    {
                        switch (item.DBFun)
                        {
                            case "count({0})":
                                DBFun = string.Format(item.DBFun, "1") + " as " + item.NewName;
                                break;
                            case "max({0})":
                            case "min({0})":
                            case "SUM({0})":
                            case "avg({0})":
                                DBFun = string.Format(item.DBFun, item.OldName) + " as " + item.OldName;
                                break;
                            default:
                                break;
                        }
                 

                    }else
                    if (i <= 0)
                    {
                        select += " "+item.NewName ;
                    }
                    else
                    {
                        select += " , "+item.NewName ;
                    }
                    i++;
                }

                sql = string.Format(sql,  select, DBFun ,TableName, select);
                SqlserverDrive driver = new SqlserverDrive();
              
                return ConverDtToEnt<TSource>(driver.ExecutiveSQL(sql, source.Parsing.ConnectionString), constructor);
            }
            return null;
        }
        #endregion

        #region OrderBy=》 Tolist -2
        public static List<TSource> ToList<TSource>(this IOrderByQUeryable<TSource> source)
        {
            string OrderBy = null, Sql = null, wherestr = null, Select = null, Count = null;


            ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();
            Type type = typeof(TSource);
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            newsource.Parsing = source.Parsing;
            string tableName = Classattr.TableName;

            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });

            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);

            }

            if (source.Parsing.Count != null)
            {

                Count = source.Parsing.Count;
            }

            if (source.Parsing.OrderByName != null && source.Parsing.OrderByName.Count > 0)
            {
                bool first = true;
                if (source.Parsing.OrderByName != null)
                    foreach (var item in source.Parsing.OrderByName)
                    {

                        if (first)
                        {
                            OrderBy += " ORDER BY   " + item.Key.ToString() + "  ";
                            first = false;
                        }
                        else
                        {
                            OrderBy += "  ,  " + item.Key.ToString() + "  ";
                        }
                        if (item.Value)
                            OrderBy += " DESC  ";
                        else OrderBy += " ASC  ";
                    }
            }



            if (source.Parsing.selectDictionary != null)
            {
                if (source.Parsing.selectDictionary.Count > 0)
                {
                    int i = 0;
                    foreach (var item in source.Parsing.selectDictionary)
                    {
                        if (i <= 0)
                            Select += item.Key.ToString() + "  AS " + item.Value.ToString();
                        else Select += " , " + item.Key.ToString() + "  AS  " + item.Value.ToString();

                        i++;
                    }
                }
            }

            string sqlcommands = "  select {0} {1} from {2}  {3}  {4}   ";
            if (Select == null) Select = " * ";
            Sql = string.Format(sqlcommands, null, Select, tableName, null, OrderBy);
            SqlserverDrive driver = new SqlserverDrive();
            return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor);
        }
        #endregion

        #region Skip=》 Tolist -1
        /// <summary>
        /// skip-->tolist
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this ISkipQUeryable<TSource> source)
        {
            ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();
            Type type = typeof(TSource);
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            newsource.Parsing = source.Parsing;
            string tableName = Classattr.TableName;
            string sqlcommand = null;
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (source == null)
            {
                throw new Exception("source");
            }
            string wherestr = null, Skip = null, Select = null, TableName = null, OrderBy = null, sql = null;
            if (source.Parsing.TableName != null)
            {
                TableName = source.Parsing.TableName;
            }



            //if (source.Parsing.Takecount > 0)
            //{
            //    Take = " Top " + source.Parsing.Takecount;            
            //}

            if (source.Parsing.TopCount > 0)
            {
                Skip = " row_number > " + source.Parsing.TopCount;
            }


            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);

            }

            if (source.Parsing.selectDictionary != null)
            {
                if (source.Parsing.selectDictionary.Count > 0)
                {
                    int i = 0;
                    foreach (var item in source.Parsing.selectDictionary)
                    {
                        if (i <= 0)
                            Select += item.Key.ToString() + "  AS " + item.Value.ToString();
                        else Select += " , " + item.Key.ToString() + "  AS  " + item.Value.ToString();

                        i++;
                    }
                }
            }


            if (source.Parsing.DBType == DatabaseType.Sqlserver)
            {

                string sqlcommandspage = "select {5} * from (select {0} {1} from {2}) as a  where {3} {4}   {6}";

                #region
                //if (Take!=null && Skip!=null)
                //{
                //    if (Select!=null)
                //    {
                //        Select += " ,row_number() OVER (ORDER BY {1} {2}) AS [row_number] ";
                //        if (source.Parsing.OrderByName.Count>0)
                //        {
                //            //source.Parsing.OrderByName.First().Key
                //        }
                //        else
                //        {
                //            //PK_Name  desc
                //        }
                //    }
                //    else
                //    {

                //       // RBaseQUeryable

                //    }
                //}
                //else
                #endregion

                if (Skip != null)
                {
                    if (Select != null)
                    {
                        Select += "   , row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                    }
                    else
                    {
                        Select += "  * , row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                    }
                    Select = string.Format(Select, source.Parsing.PK_Name.ToString(), "Desc");
                    sql = string.Format(sqlcommandspage, null, Select, TableName, "a.[row_number]>", source.Parsing.TopCount, null, null);

                }
                // string.Format(sqlcommands,null);

                //  string sqlcommands2 = "select ";
                //  string obderby = "order by ";

                SqlserverDrive driver = new SqlserverDrive();


                return ConverDtToEnt<TSource>(driver.ExecutiveSQL(sql, source.Parsing.ConnectionString), constructor);


            }

            return null;
        }
        #endregion

        #region  Take=>Tolist 0
        public static List<TSource> ToList<TSource>(this ITakeQUeryable<TSource> source)
        {
            string wherestr = null, Take = null, Select = null, TableName = null, Sql = null;

            #region
            //var PKColumnquer = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
            //{
            //    var attr = p.GetCustomAttributes(typeof(ColumnAttribute), false).Where(PP =>
            //((ColumnAttribute)PP).Primary

            //);
            //    if (attr.Count() > 0)
            //        return true;
            //    else if (new string[] { "ID", "id", "Id", "iD" }.Contains(p.Name))
            //        return true;
            //    return false;

            //}).ToList();
            //source.Parsing.PK_Name = PKColumnquer.First().Name.ToString();

            #endregion

            ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();
            TableName = GetTabName(typeof(TSource));
  
            string sqlcommand = null;
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (source == null)
            {
                throw new Exception("source");
            }

            if (source.Parsing.TableName != null)
            {
                TableName = source.Parsing.TableName;
            }



            if (source.Parsing.Takecount > 0)
            {
                Take = " Top " + source.Parsing.Takecount;
            }

            //if (source.Parsing.TopCount > 0)
            //{
            //    Skip = " row_number > " + source.Parsing.TopCount;
            //}


            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);

            }



            if (source.Parsing.selectDictionary != null && source.Parsing.selectDictionary.Count > 0)
            {
                int i = 0;
                foreach (var item in source.Parsing.selectDictionary)
                {
                    if (i <= 0)
                        Select += item.Key.ToString() + "  AS " + item.Value.ToString();
                    else Select += " , " + item.Key.ToString() + "  AS  " + item.Value.ToString();

                    i++;
                }
            }



            if (source.Parsing.DBType == DatabaseType.Sqlserver)
            {
                string sqlcommandspage = "  select {0} {1}  from {2} as a where {3}  {4}  {5}   ";
                // string sqlcommandspage = "select {5} * from (select {0} {1} from {2}) as a  where {3} {4}   {6}";


                if (source.Parsing.TopCount > 0)
                {
                    Take = " Top " + source.Parsing.TopCount;
                }

                #region
                if (Take != null)
                {
                    if (Select == null)
                    {
                        Select += "  *  ";
                    }

                   
                     source.Parsing.PK_Name = GetPKName(typeof(TSource));
                    Select = string.Format(Select, source.Parsing.PK_Name.ToString(), "Desc");

                    if (wherestr == null) wherestr = " 1=1 ";

                    Sql = string.Format(sqlcommandspage, Take, Select, TableName, wherestr, null, null);

                }
                #endregion

                SqlserverDrive driver = new SqlserverDrive();


                return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor);


            }

            return null;
        }
        #endregion

        #region ToList1
        public static List<TSource> ToList<TSource>(this ISelectQUeryable<TSource> source)
        {
            if (source == null)
            {
                throw new Exception("source");
            }

            string wherestr = null, Skip = null, Take = null, Select = null, TableName = null, OrderBy = null, Sql = null, DBFun=null;
            if (source.Parsing.TableName != null)
            {
                TableName = source.Parsing.TableName;
            }
            else
            {
                throw new Exception("source");
            }
            if (source.Parsing.TopCount > 0)
            {
                Take = " Top " + source.Parsing.TopCount;
            }

            if (source.Parsing.Takecount > 0)
            {
                Skip = " [row_number] > " + source.Parsing.Takecount;
            }
            if (source.Parsing.selectDictionary != null)
            {
                if (source.Parsing.selectDictionary.Count > 0)
                {
                    int i = 0;
                    foreach (var item in source.Parsing.selectDictionary)
                    {
                        if (i <= 0)
                        {
                            Select += item.Key.ToString();

                        }
                        else { Select += " , " + item.Key.ToString(); }
                
                        i++;
                    }
                }
                else
                {

                    Select = " * ";
                }
            }

            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);

            }

            if (source.Parsing.OrderByName != null && source.Parsing.OrderByName.Count > 0)
            {
                bool first = true;
                if (source.Parsing.OrderByName != null)
                    foreach (var item in source.Parsing.OrderByName)
                    {

                        if (first)
                        {
                            OrderBy += " ORDER BY   " + item.Key.ToString() + "  ";
                            first = false;
                        }
                        else
                        {
                            OrderBy += "  ,  " + item.Key.ToString() + "  ";
                        }
                        if (item.Value)
                            OrderBy += " DESC  ";
                        else OrderBy += " ASC  ";
                    }
            }

            #region   GroupBy
            if (source.Parsing.GroupByList != null && source.Parsing.GroupByList.Count > 0) 
            {


                int i = 0;
                foreach (var item in source.Parsing.GroupByList)
                {
                    if (item.DBFun != null)
                    {
                        switch (item.DBFun)
                        {
                            case "count({0})":
                                DBFun = string.Format(item.DBFun, "1") + " as " + item.NewName;
                                break;
                            case "max({0})":
                            case "min({0})":
                            case "SUM({0})":
                            case "avg({0})":
                                DBFun = string.Format(item.DBFun, item.OldName) + " as " + item.OldName;
                                break;
                            default:
                                break;
                        }


                    }
                    else
                        if (i <= 0)
                        {
                           // select += " " + item.NewName;
                        }
                        else
                        {
                           // select += " , " + item.NewName;
                        }
                    i++;
                }


            }
            #endregion

            if (source.Parsing.DBType == DatabaseType.Sqlserver)
            {
                string sqlcommands = "select {0} {1} from {2}  where {3} {4}";
                string sqlcommand = "select {0} {1} from {2}";
                string sqlcommandspage = "select {5} * from (select {0} {1} from {2}) as a  where {3} {4})  {6}";

                if (Take != null && Skip != null)
                {
                    if (Select != null)
                    {
                        Select += "  ,row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                        if (source.Parsing.OrderByName != null && source.Parsing.OrderByName.Count > 0)
                        {
                            Select = string.Format(Select, source.Parsing.PK_Name.ToString(), "Desc");
                            Sql = string.Format(sqlcommandspage, "*,", Select, TableName, "a.[row_number]>", source.Parsing.TopCount.ToString() + "*" + "(" + source.Parsing.Takecount.ToString() + "- 1", Take, OrderBy);
                            //source.Parsing.OrderByName.First().Key
                        }
                        else
                        {
                            //PK_Name  desc
                            Select += "  , row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                            Select = string.Format(Select, source.Parsing.PK_Name.ToString(), "Desc");
                            Sql = string.Format(sqlcommandspage, "*,", Select, TableName, "a.[row_number]>", source.Parsing.TopCount.ToString() + "*" + "(" + source.Parsing.Takecount.ToString() + "- 1", Take, null);
                        }
                    }
                    else
                    {
                        Select += "  *  , row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                        Select = string.Format(Select, source.Parsing.PK_Name.ToString(), "Desc");
                        Sql = string.Format(sqlcommandspage, "*,", Select, TableName, "a.[row_number]>", source.Parsing.TopCount.ToString() + "*" + "(" + source.Parsing.Takecount.ToString() + "- 1", Take, OrderBy);
                    }
                }
                else if (Skip != null)
                {
                    if (Select != null)
                    {
                        Select += "   , row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                    }
                    else
                    {
                        Select += "  * , row_number() OVER (ORDER BY {0} {1}) AS [row_number]  ";
                    }
                    Select = string.Format(Select, source.Parsing.PK_Name.ToString(), "Desc");
                    Sql = string.Format(sqlcommandspage, "*,", Select, TableName, "a.[row_number]>", source.Parsing.TopCount.ToString(), null, null);
                }
                if (wherestr != null)
                {
                    Sql = string.Format(sqlcommands, Take, Select, TableName, wherestr, OrderBy);
                }
                else
                {
                    Sql = string.Format(sqlcommand, Take, Select, TableName);
                }





                SqlserverDrive driver = new SqlserverDrive();

                return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), source.Parsing.constructor);

            }

            return null;
        }
        #endregion

        #region ToList2
        /// <summary>
        /// tolist
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this IQUeryable<TSource> source)
        {
            //TSource aa = new TSource();
            ISelectQUeryable<TSource> newsource = new RBaseQUeryable<TSource>();

            string tableName = GetTabName(typeof(TSource));
            //object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            //TableAttribute Classattr = objs as TableAttribute;
            //newsource.Parsing = source.Parsing;
            //string tableName = Classattr.TableName;
            string sqlcommand = null;
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (constructor.IsNull())
            {
                throw new Exception("");
            }
            else
            {


                if (source == null)
                {
                    throw new Exception("source");
                }

                if (source.Parsing.DBType == DatabaseType.Sqlserver)
                {
                    sqlcommand = "select * from {0}";
                    //string sqlcommands2 = "select ";
                    //string obderby = "order by ";

                }
                string Sql = string.Format(sqlcommand, tableName);
                SqlserverDrive driver = new SqlserverDrive();
                return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor);
                //if (source.Parsing.WhereExpression is BinaryExpression)
                //{
                //    BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                //    string wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);
                //    if (source.Parsing.DBType == DatabaseType.Sqlserver)
                //    {

                //        //string sqlcommands2 = "select ";
                //        //string obderby = "order by ";

                //    }
                //}
                //else
                //{
                //    throw new Exception("Where No BinaryExpression");
                //}

            }
            // return null;

        }






        #endregion
        #endregion


        #region Count

        #region  QUeryable=>count
        public static int Count<TSource>(this IQUeryable<TSource> source)
        {

            String TableName = GetTabName(typeof(TSource));
            String Sql = null;

            if (source == null)
            {
                throw new Exception("source");
            }
            source.Parsing.Count = " count(1)  ";

            Sql = "  select {0} {1}  from {2} {3} {4}  ";

            Sql = string.Format(Sql, null, " count(1) ", TableName, null, null, null);


            SqlserverDrive driver = new SqlserverDrive();
            int count = driver.ExecuteScalar(Sql, source.Parsing.ConnectionString);
            return count;

            #region
            //if (source.Parsing.DBType == DatabaseType.Sqlserver)
            //{

            //    // if (source is BinaryExpression)
            //    //  {
            //    BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
            //    string sr = s.Right.ToString();



            //    // }



            //}
            #endregion
        }
        #endregion

        #region  QUeryable=>count
        public static int Count<TSource>(this IWhereQUeryable<TSource> source)
        {
            String TableName = GetTabName(typeof(TSource));
            String Sql = " select  {0} from {1} where {2} ", wherestr = null;
            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);
            }
            Sql = string.Format(Sql, " count(1) ", TableName, wherestr);
            SqlserverDrive driver = new SqlserverDrive();
            int count = driver.ExecuteScalar(Sql, source.Parsing.ConnectionString);
            return count;
        }
        #endregion

        #region  Count
        //public static int Count<TSource>(this ISelectQUeryable<TSource> source)
        //{
        //    String TableName = GetTabName(typeof(TSource)), Select=null;
        //    if (source.Parsing.selectDictionary != null && source.Parsing.selectDictionary.Count > 0)
        //    {
        //        int i = 0;
        //        foreach (var item in source.Parsing.selectDictionary)
        //        {
        //            if (i <= 0)
        //                Select += item.Key.ToString() + "  AS " + item.Value.ToString();
        //            else Select += " , " + item.Key.ToString() + "  AS  " + item.Value.ToString();

        //            i++;
        //        }
        //    }

        //    string Sql = "  select {0} {} from  {1}  {2}   ";
        //    //string.Format(Sql,);
        //    return 0;
        //}

        #endregion

        #endregion

        #region  Frist

        public static TSource FirstOrDefault<TSource>(this IQUeryable<TSource> source)
        {

            string Sql = " select Top 1 from  {0} {1} ";
            string TabName = GetTabName(typeof(TSource));
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (constructor.IsNull())
            {
                throw new Exception("");
            }
            string.Format(Sql, TabName, null);
            SqlserverDrive driver = new SqlserverDrive();
            return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor)[0];

        }

        public static TSource FirstOrDefault<TSource>(this IWhereQUeryable<TSource> source)
        {
            string Sql = " select Top 1 from  {0} {1} ", wherestr = null;
            string TabName = GetTabName(typeof(TSource));
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (constructor.IsNull())
            {
                throw new Exception("");
            }
            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);
            }
            string.Format(Sql, TabName, wherestr);
            SqlserverDrive driver = new SqlserverDrive();
            return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor)[0];
        }

        public static TSource FirstOrDefault<TSource>(this ISelectQUeryable<TSource> source)
        {
            string Sql = " select Top 1 {0} from  {1} {2} ", wherestr = null, Select = null;
            string TabName = GetTabName(typeof(TSource));
            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (constructor.IsNull())
            {
                throw new Exception("");
            }
            if (source.Parsing.WhereExpression is BinaryExpression)
            {
                BinaryExpression s = (BinaryExpression)source.Parsing.WhereExpression;
                wherestr = BinaryExpressionHandler(s.Left, s.Right, s.NodeType);
            }
            if (source.Parsing.selectDictionary != null)
            {
                if (source.Parsing.selectDictionary.Count > 0)
                {
                    int i = 0;
                    foreach (var item in source.Parsing.selectDictionary)
                    {
                        if (i <= 0)
                        {
                            Select += item.Key.ToString();
                        }
                        else { Select += " , " + item.Key.ToString(); }
                        i++;
                    }
                }
                else
                {
                    Select = " * ";
                }
            }
            Sql = string.Format(Sql, Select, TabName, wherestr);
            SqlserverDrive driver = new SqlserverDrive();
            return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor)[0];
        }

        #endregion

        #region   Last
        public static TSource LastOrDefault<TSource>(this IQUeryable<TSource> source)
        {

            string Sql = "select * from ( select {0},row_number() over( ORDER BY {1} DESC)  as rw FROM {2} ) t  where t.rw=1 ";
            string wherestr = null, Select = null;
            string TabName = GetTabName(typeof(TSource));
 
            source.Parsing.PK_Name = GetPKName(typeof(TSource));

            ConstructorInfo constructor = typeof(TSource).GetConstructor(new Type[] { });
            if (constructor.IsNull())
            {
                throw new Exception("");
            }
            string.Format(Sql, Select, source.Parsing.PK_Name, TabName, wherestr);
            SqlserverDrive driver = new SqlserverDrive();
            return ConverDtToEnt<TSource>(driver.ExecutiveSQL(Sql, source.Parsing.ConnectionString), constructor)[0];

        }

        #endregion

        #region BinaryExpressionHandler 拆分表达式树
        /// <summary>
        /// 拆分表达式树
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string BinaryExpressionHandler(Expression left, Expression right, ExpressionType type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            string needParKey = "=,>,<,>=,<=,<>";
            string leftPar = RouteExpressionHandler(left);
            string typeStr = ExpressionTypeCast(type);
            var isRight = needParKey.IndexOf(typeStr) > -1;
            string rightPar = RouteExpressionHandler(right, isRight);


            string appendLeft = leftPar;

            sb.Append(appendLeft);//字段名称

            if (rightPar.ToUpper() == "NULL")
            {
                if (typeStr == "=")
                    rightPar = " IS NULL ";
                else if (typeStr == "<>")
                    rightPar = " IS NOT NULL ";
            }
            else
            {
                sb.Append(typeStr);
            }
            sb.Append(rightPar);
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region RouteExpressionHandler 解析表达式

        public static string GetGenericArryString<T>(object value)
        {

            T[] list = (T[])value;
            bool first = true;
            StringBuilder querstr = new StringBuilder();
            foreach (var item in list)
            {
                if (!first)
                {
                    querstr.Append(",");
                }
                else
                {
                    first = false;
                }
                querstr.Append("'" + item + "'");
            } 
            return querstr.ToString();
        }
        public static string GetGenericString<T>(object value)
        {

            IEnumerable<T> list = (IEnumerable<T>)value;
            bool first = true;
            StringBuilder querstr = new StringBuilder();
            foreach (var item in list)
            {
                if (!first)
                {
                    querstr.Append(",");
                }
                else
                {
                    first = false;
                }
                querstr.Append("'" + item + "'");
            } 
            return querstr.ToString();
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isRight"></param>
        /// <returns></returns>
        public static string RouteExpressionHandler(Expression exp, bool isRight = false)
        {
            if (exp is BinaryExpression)
            {
                BinaryExpression be = (BinaryExpression)exp;
                //重新拆分树,形成递归
                return BinaryExpressionHandler(be.Left, be.Right, be.NodeType);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression mExp = (MemberExpression)exp;
                if (isRight)//按表达式右边值
                {
                    var obj = Expression.Lambda(mExp).Compile().DynamicInvoke();
                    if (obj is Enum)
                    {
                        obj = (int)obj;

                    }
                    else if (obj is string)
                    {
                        obj = "'" + obj + "'";
                    }
                    return obj.ToString();
                }
                return mExp.Member.Name;
            }
            else if (exp is NewArrayExpression)
            {
                #region 数组
                NewArrayExpression naExp = (NewArrayExpression)exp;
                StringBuilder sb = new StringBuilder();
                foreach (Expression expression in naExp.Expressions)
                {
                    sb.AppendFormat(",{0}", RouteExpressionHandler(expression));
                }
                return sb.Length == 0 ? "" : sb.Remove(0, 1).ToString();
                #endregion
            }
            else if (exp is MethodCallExpression)
            {
                if (isRight)
                {
                    return Expression.Lambda(exp).Compile().DynamicInvoke() + "";
                }
                //在这里解析方法
                string sqlin = "{0} in ({1})";

                MethodCallExpression methodcall = exp as MethodCallExpression;
             

                if (methodcall.Method.Name.StartsWith("ContainsMt"))
                {


                    var Argumentslambda = methodcall.Arguments[0];
                    var quer = Expression.Lambda(Argumentslambda).Compile().DynamicInvoke();
                     
                    Type querType = quer.GetType();

                    string inlist = string.Empty, ContainsMtArgumentstr = string.Empty;

                    if (querType.BaseType.Equals(typeof(Array)))
                    {
                        Type t = Type.GetType(querType.FullName.Replace("[", "").Replace("]", ""));
                        inlist = typeof(QUeryable).GetMethod("GetGenericArryString").MakeGenericMethod(new Type[] { t }).Invoke(null, new object[] { quer }).ToString();
                    }
                    else
                    {
                        Type t = querType.GetGenericArguments().FirstOrDefault();
                        inlist = typeof(QUeryable).GetMethod("GetGenericString").MakeGenericMethod(new Type[] { t }).Invoke(null, new object[] { quer }).ToString();
                    }
                  
                    if (methodcall.Arguments[1] is UnaryExpression)
                    {
                        UnaryExpression unary = methodcall.Arguments[1] as UnaryExpression;
                        if (unary.Operand is MemberExpression)
                        {
                            ContainsMtArgumentstr = ((MemberExpression)unary.Operand).Member.Name;
                        }
                    }
                    else if (methodcall.Arguments[1] is MemberExpression)
                    {
                        ContainsMtArgumentstr = ((MemberExpression)methodcall.Arguments[1]).Member.Name;
                    }
                    else
                    {
                        throw new Exception("错误的表达式！");
                    }


                    return string.Format(sqlin, ContainsMtArgumentstr, inlist);
                }



                throw new Exception("暂不支持");
            }
            else if (exp is ConstantExpression)
            {
                #region 常量
                ConstantExpression cExp = (ConstantExpression)exp;
                if (cExp.Value == null)
                    return "null";

                return ValueConvert(cExp.Value);
                #endregion
            }
            else if (exp is UnaryExpression)
            {

                UnaryExpression ue = ((UnaryExpression)exp);
                return RouteExpressionHandler(ue.Operand, isRight);
            }
            return null;
        }
        #endregion

        #region  ValueConvert
        public static string ValueConvert(object Value)
        {
            var val = Value.ToString();
            if (Value.GetType().IsValueType)
            {
                return val;
            }
            else if (Value is Enum)
            {
                return val;
            }
            else
            {
                return "'" + val + "'";
            }
        }
        #endregion

        #region ConverDtToEnt
        public static List<TSource> ConverDtToEnt<TSource>(DataTable data, ConstructorInfo constructors)
        {
            if (constructors != null)
            {

                string tempName = string.Empty;

                var model = constructors.Invoke(null);

                List<TSource> ts = new List<TSource>();
                foreach (DataRow dr in data.Rows)
                {

                    if (model.IsNull())
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        PropertyInfo[] propertys = typeof(TSource).GetProperties();// model.GetType().GetProperties();

                        foreach (PropertyInfo pi in propertys)
                        {
                            tempName = pi.Name;
                            //检查DataTable是否包含此列（列名==对象的属性名）  
                            if (data.Columns.Contains(tempName))
                            {
                                //该属性不可写，直接跳出
                                if (!pi.CanWrite) continue;
                                object value = dr[tempName];
                                if (value != DBNull.Value)

                                    pi.SetValue(model, value, null);
                            }
                        }

                        ts.Add((TSource)model);
                    }

                }

                return ts;


            }
            else
            {
                List<TSource> ts = new List<TSource>();
                foreach (DataRow dr in data.Rows)
                {
                    ts.Add((TSource)dr[0]);
                }
                return ts;
            }
        }

        #endregion

        #region  ExpressionTypeCast 运算符转换
        /// <summary>
        /// 运算符转换
        /// </summary>
        /// <param name="expType"></param>
        /// <returns></returns>
        public static string ExpressionTypeCast(ExpressionType expType)
        {
            switch (expType)
            {
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    throw new InvalidCastException("不支持的运算符");
            }
        }
        #endregion


        #region 转换
        private static string GetTabName(Type type)
        {
            // Type type = typeof(TSource);
            string tableName = null;
            object objs = type.GetCustomAttribute(typeof(TableAttribute), true);
            TableAttribute Classattr = objs as TableAttribute;
            if (Classattr != null)
            {
                tableName = Classattr.TableName;

            }
            else
            {
                tableName = type.Name;
                //foreach (var item in type.GetProperties())
                //{
                //    tableName = item.Name;
                //    break;

                //}
            }

            return tableName;

        }
        private static string GetPKName(Type type)
        {
            var PKColumnquer = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
            {
                var attr = p.GetCustomAttributes(typeof(ColumnAttribute), false).Where(PP =>
            ((ColumnAttribute)PP).Primary

            );
                if (attr.Count() > 0)
                    return true;
                else if (new string[] { "ID", "id", "Id", "iD" }.Contains(p.Name))
                    return true;
                return false;

            }).ToList();
            return  PKColumnquer.First().Name.ToString();

        }
        #endregion

    }
}






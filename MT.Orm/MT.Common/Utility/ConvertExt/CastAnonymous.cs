using System.Collections.Generic;

namespace MT.Common.Utility.ConvertExt
{
    public static class CastAnonymous
    {
        /// <summary>
        /// 匿名类转换
        /// </summary>
        /// <param name="anonymousType"></param>
        /// <param name="anonymous"></param>
        /// <returns></returns>  
        public static List<T> CastAnonymousList<T>(this List<object> anonymous, T defaultValue)
        {
            List<T> retlist = new List<T>();
            foreach ( var item in anonymous )
            {
                retlist.Add((T) item);
            }
            return retlist;
        }

    }
}

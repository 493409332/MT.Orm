using MT.Common.Utility.CacheExt;
using System;

namespace MT.Complex.Logical.Admin.CacheManag
{
    public class CacheManagement
    {
        public static readonly CacheManagement Instance = new CacheManagement();
        //分钟
        const double timeout = 30;
        public void Add(string key, object value)
        {
            MyCache.Instance.Add(key, value, DateTime.Now.AddMinutes(timeout));
        }

        public void Add(string modelname, int ID, object value)
        {
            MyCache.Instance.Add(modelname + "&" + ID, value, DateTime.Now.AddMinutes(timeout));
        }

        public void RemoveByID(string modelname, int ID)
        {
            MyCache.Instance.Remove(modelname + "&" + ID);
        }

        public void RemoveStartsWith(string modelname)
        {
            MyCache.Instance.RemoveStartsWith(modelname);
        }
        public T Get<T>(string key) where T : class
        {
            return MyCache.Instance.Get<T>(key);
        }

        public T Get<T>(string modelname, int ID) where T : class
        {
            return MyCache.Instance.Get<T>(modelname + "&" + ID);
        }
        //MyCache.Instance.Get<T_UserInfo>("T_UserInfo&" + ID)
    }
}

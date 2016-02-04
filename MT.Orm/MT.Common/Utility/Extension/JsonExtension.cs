using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MT.Common.Utility.Extension
{
    public static class JsonExtension
    {
        /// <summary>
        /// 序列化操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string Serialize<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="json">字符传</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 序列化小写属性名json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeCamelCasePropertyNamesLower<T>(this T obj) where T : class
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CasePropertyNamesLowerContractResolver()
            };
            return JsonConvert.SerializeObject(obj, settings); 
        
        }
        public class CasePropertyNamesLowerContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            } 
        }
    }
}

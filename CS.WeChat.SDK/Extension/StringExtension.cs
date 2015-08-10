using Newtonsoft.Json;

namespace System
{
    public static class StringExtension
    {
        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
           return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
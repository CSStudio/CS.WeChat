using System;
using Newtonsoft.Json;

namespace System
{
    /// <summary>
    /// 序列化成JSON字符串
    /// </summary>
    public static class ObjectExtension
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
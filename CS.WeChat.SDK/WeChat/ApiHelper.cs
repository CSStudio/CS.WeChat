using System;
using System.Globalization;
using CS.Cryptography;

namespace CS.WeChat
{
    public class ApiHelper
    {
        private static readonly Random random;
        private ApiHelper()
        {
        }

        static ApiHelper()
        {
            random = new Random();
        }



        /// <summary>
        /// 获取AppId对应的JS-SDK权限验证的签名Signature
        /// <remarks>
        /// 签名生成规则如下：参与签名的字段包括noncestr（随机字符串）, 有效的jsapi_ticket, timestamp（时间戳）, url（当前网页的URL，不包含#及其后面部分） 。
        /// 对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）后，使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串string1。
        /// 这里需要注意的是所有参数名均为小写字符。对string1作sha1加密，字段名和字段值都采用原始值，不进行URL 转义。
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public static string GetJsSignature(string appId ,string timeStamp,string randomString,string currUrl)
        {
            var ticket = AppContainer.GetJsTicket(appId);
            var source =
                $"jsapi_ticket={ticket}&noncestr={randomString}&timestamp={timeStamp}&url={currUrl}";
            return Sha1.Encrypt(source);
        }

        public static string GetTimestamp()
        {
            return DateTime.Now.ToSecondTime().ToString();
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandomString()
        {
            return Md5.Encrypt(random.NextDouble().ToString(CultureInfo.InvariantCulture));
        }



    }
}
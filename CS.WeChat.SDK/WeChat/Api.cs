using System;
using CS.Diagnostics;
using CS.Http;
using Newtonsoft.Json;

namespace CS.WeChat
{
    /// <summary>
    /// 微信的API入口
    /// </summary>
    public class Api
    {

        private Api()
        {
        }

        /// <summary>
        /// 获取默认的AccessToken
        /// </summary>
        public static string AccessToken => AppContainer.AccessToken;


        public static TokenResult GetToken(string appId, string appSecret)
        {
            return RequestToken(appId, appSecret).FromJson<TokenResult>();
        }

        public static string RequestToken(string appId, string appSecret)
        {
            var url = $"{ApiHost.API}cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";
            var res = HttpHelper.Get(url);
            Tracer.Debug($"RequestToken\nRequest:{url}\nResponse:{res}");
            return res;
        }

        //access_token是公众号的全局唯一票据，公众号调用各接口时都需使用access_token。开发者需要进行妥善保存。access_token的存储至少要保留512个字符空间。access_token的有效期目前为2个小时，需定时刷新，重复获取将导致上次获取的access_token失效。
    }

    public class TokenResult : ReturnResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class ReturnResult
    {
        [JsonProperty("errcode")]
        public int Errcode { get; set; }

        [JsonProperty("errmsg")]
        public string Errmsg { get; set; }
    }
}


using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using CS.Caching;
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
        public static string BaseUrl = "https://api.weixin.qq.com/";

        private static ConcurrentDictionary<string, TokenItem> dic = new ConcurrentDictionary<string, TokenItem>();

        static Api()
        {
            //TODO:通过配置文件配置AppId
        }

        /// <summary>
        /// 使用微信AppId及AppSecret来初始化
        /// <remarks>
        /// 多套AppId时初始化多次即可
        /// </remarks>
        /// </summary>
        public static void Init(string appId, string appSecret)
        {
            dic[appId] = new TokenItem() {Key = appId, AppSecret = appSecret};
        }

        /// <summary>
        /// 获取默认的AccessToken
        /// </summary>
        public static string AccessToken => GetAccessToken(null);

        /// <summary>
        /// 根据获取AccessToken
        /// </summary>
        /// <param name="appId">null时返回第一个有效的AccessToken</param>
        /// <returns></returns>
        public static string GetAccessToken(string appId)
        {
            TokenItem item;
            if (string.IsNullOrEmpty(appId))
            {
                item = dic.FirstOrDefault().Value;
                appId = dic.FirstOrDefault().Key;
            }
            else
            {
                dic.TryGetValue(appId, out item);
            }
            if (item == null) throw new Exception("请先执行Api.Init 方法初始化至少一个应用的App信息");
            var now = DateTime.Now.ToSecondTime();
            if (item.Value == null || item.ExpiressTime <= now)
            {
                item.Value = GetToken(appId, item.AppSecret);
                item.ExpiressTime = now + item.Value.ExpiresIn - 3; //5秒误执行纠正
            }
            return item.Value.AccessToken;
        }


        public static TokenResult GetToken(string appId, string appSecret)
        {
            return RequestToken(appId, appSecret).FromJson<TokenResult>();
        }

        public static string RequestToken(string appId, string appSecret)
        {
            var url = BaseUrl + $"cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";
            var res = HttpHelper.Get(url);
            Tracer.Debug($"RequestToken\nRequest:{url}\nResponse:{res}");
            return res;
        }

        //access_token是公众号的全局唯一票据，公众号调用各接口时都需使用access_token。开发者需要进行妥善保存。access_token的存储至少要保留512个字符空间。access_token的有效期目前为2个小时，需定时刷新，重复获取将导致上次获取的access_token失效。
    }

    internal class TokenItem : CacheItem<string, TokenResult>
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppSecret { get; set; }
      
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


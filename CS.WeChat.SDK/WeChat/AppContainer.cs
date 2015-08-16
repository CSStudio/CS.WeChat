using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using CS.Caching;
using CS.Config;
using CS.Diagnostics;

namespace CS.WeChat
{
    /// <summary>
    /// 微信平台相关参数缓存容器
    /// </summary>
    public class AppContainer
    {
        private AppContainer()
        {
        }

        static AppContainer()
        {
            Dic = new ConcurrentDictionary<string, AppItem>
            {
                [WeChatSetting.AppId] = new AppItem(WeChatSetting.AppId, WeChatSetting.AppSecret)
                {
                    AuthScope = WeChatSetting.AuthScope ,
                    OAuthCallbackUrl = WeChatSetting.OAuthCallbackUrl
                }
            }; //至少有一个App应用
        }

        private static readonly ConcurrentDictionary<string, AppItem> Dic ;

        public static void Init(string appId, string appSecret)
        {
            Dic[appId] = new AppItem(appId, appSecret);
        }

        /// <summary>
        /// 返回默认的Token
        /// </summary>
        public static string AccessToken => GetAccessToken();
        /// <summary>
        /// 返回默认的JsTicket
        /// </summary>
        public static string JsTicket => GetJsTicket();


        /// <summary>
        /// 返回对应的AppId的Token
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string GetAccessToken(string appId=null)
        {
            var item = GetAppItem(appId);
            var now = DateTime.Now.ToSecondTime();
            if (item.TokenTimeout < now)
            {
                //更新缓存数据
                var res = Api.GetToken(item.AppId, item.AppSecret);
                if (res.Errcode == 0)
                {
                    item.AccessToken = res.AccessToken;
                    item.TokenTimeout = DateTime.Now.ToSecondTime() + res.ExpiresIn;
                }
                else
                {
                    Tracer.Error($"获取Token出错：errcode:{res.Errcode};errmsg:{res.Errmsg}");
                }
            }
            return item.AccessToken;
        }

        /// <summary>
        /// 返回对应的AppId的JsTicket
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string GetJsTicket(string appId=null)
        {
            var item = GetAppItem(appId);
            var now = DateTime.Now.ToSecondTime();
            if (item.TiecktTimeout < now)
            {
                //更新缓存数据
                var token = GetAccessToken(appId);
                var res = JsApi.GetTicket(token);
                if (res.Errcode == 0)
                {
                    item.JsTicket = res.Ticket;
                    item.TiecktTimeout = DateTime.Now.ToSecondTime() + res.ExpiresIn;
                }
                else
                {
                    Tracer.Error($"获取Ticket出错：errcode:{res.Errcode};errmsg:{res.Errmsg}");
                }
            }
            return item.JsTicket;
        }

        /// <summary>
        /// 获取AppItem，如果AppId为空时则获取默认的
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        internal static AppItem GetAppItem(string appId = null)
        {
            AppItem item;
            if (string.IsNullOrEmpty(appId))
            {
                item = Dic.FirstOrDefault().Value;
            }
            else
            {
                Dic.TryGetValue(appId, out item);
            }
            if (string.IsNullOrEmpty(item?.AppSecret)) throw new OperationCanceledException($"please init the wechat app with appId=[{appId}] & appSecret first.");
            return item;
        }

        /// <summary>
        /// 配置中默认的AppId
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultAppId()
        {
            var item = GetAppItem();
            return item.AppId;
        }

        /// <summary>
        /// 返回JsSdk中的Js的相关参数
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="absUri"></param>
        /// <returns></returns>
        public static dynamic GetJsSdkSetting(string absUri ,string appId = null)
        {
            dynamic json = new ExpandoObject();
            json.appId = string.IsNullOrEmpty(appId)? GetDefaultAppId() : appId;
            json.timestamp = ApiHelper.GetTimestamp();
            json.nonceStr = ApiHelper.GetRandomString();
            json.signature = ApiHelper.GetJsSignature(json.appId, json.timestamp, json.nonceStr, absUri);
            return json;
        }


    }

    /// <summary>
    /// 项目缓存
    /// </summary>
    internal class AppItem
    {
        public AppItem(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
            TokenTimeout = 0;
            TiecktTimeout = 0;
        }

        /// <summary>
        /// 
        /// </summary>

        public string AppId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string AppSecret { get; }

        /// <summary>
        /// Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Token过期时间，秒
        /// </summary>
        public long TokenTimeout { get; set; }

        /// <summary>
        /// JS_SDK用到的临时票据Ticket
        /// </summary>
        public string JsTicket { get; set; }

        /// <summary>
        /// 票据过期时间，秒
        /// </summary>
        public long TiecktTimeout { get; set; }



        public string AuthScope { get; set; }

        public string OAuthCallbackUrl { get; set; }

    }
}
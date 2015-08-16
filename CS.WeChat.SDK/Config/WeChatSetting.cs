using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;

namespace CS.Config
{
    public class WeChatSetting
    {
        private WeChatSetting()
        {
        }

        public static string AppId => WebChatConfig.Instance.KeyValues["appId"];

        public static string AppSecret => WebChatConfig.Instance.KeyValues["appSecret"];

        /// <summary>
        /// 授权方式 snsapi_base, snsapi_userinfo(要用户手动确认)
        /// </summary>
        public static string AuthScope => WebChatConfig.Instance.KeyValues["AuthScope"].ToInt(0) == 0 ? "snsapi_base" : "snsapi_userinfo";

        /// <summary>
        /// 页面授权时的回调地址
        /// </summary>
        public static string OAuthCallbackUrl => WebChatConfig.Instance.KeyValues["OAuthCallbackUrl"];

    }

    class WebChatConfig : SectionBase
    {
        private WebChatConfig() : base("appWeChat")
        {
        }

        private static WebChatConfig instance;
        private static readonly object _lock = new object();

        public static WebChatConfig Instance
        {
            get
            {
                if (
            instance != null)
                    return instance;
                lock (_lock)
                {
                    if (instance != null) return instance;
                    instance = new WebChatConfig();
                }
                return instance;
            }
        }
        
    }


}
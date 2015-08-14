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
using System;
using System.Collections.Concurrent;
using System.Linq;
using CS.Caching;
using CS.Config;
using CS.Diagnostics;
using CS.Http;
using Newtonsoft.Json;

namespace CS.WeChat
{
    /// <summary>
    /// 微信的JS-SDK的API入口
    /// </summary>
    public class JsApi
    {

        private JsApi()
        {
        }

        /// <summary>
        /// 获取默认的Js-SDK的临时Ticket
        /// </summary>
        public static string JsTicket => AppContainer.GetJsTicket(null);

        public static TicketResult GetTicket(string token)
        {
            return RequestTicket(token).FromJson<TicketResult>();
        }

        public static string RequestTicket(string token)
        {
            var url = $"{ApiUrl.BaseUrl}cgi-bin/ticket/getticket?access_token={token}&type=jsapi";
            var res = HttpHelper.Get(url);
            Tracer.Debug($"RequestToken\nRequest:{url}\nResponse:{res}");
            return res;
        }

    }



    internal class TicketItem : CacheItem<string, TicketResult>
    {
    }

    public class TicketResult : ReturnResult
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

}
using System;
using System.Web;
using CS.Diagnostics;
using CS.Http;

namespace CS.WeChat
{
    /// <summary>
    /// OAuth授权获取用户信息
    /// </summary>
    public class OAuthApi
    {
        private OAuthApi()
        {
        }



        public static void RequestAuthorize(string appId=null, string state=null)
        {
            var appItem = AppContainer.GetAppItem(appId);
            RequestAuthorize(appItem.AppId,appItem.OAuthCallbackUrl,appItem.AuthScope,state);
        }

        /// <summary>
        /// 第一步：用户同意授权，获取code
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="callbackUrl">授权后重定向的回调链接地址，使用urlencode对链接进行处理</param>
        /// <param name="authScore">应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节</param>
        /// <returns></returns>
        public static void RequestAuthorize(string appId,string callbackUrl,string authScore,string state)
        {
            var url =
                $"{ApiHost.OPEN}connect/oauth2/authorize?appid={appId}&redirect_uri={callbackUrl.EncodeUrl()}&response_type=code&scope={authScore}&state={state}#wechat_redirect";
            Tracer.Debug($"RequestAuthorize\nRequest:{url}");
            HttpContext.Current.Response.Redirect(url, false);
            HttpContext.Current.Response.End();
        }



        /// <summary>
        /// 第二步：通过code换取网页授权access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string RequestAccessToken(string appId, string appSecret,string code)
        {
            var url =
                $"{ApiHost.API}/sns/oauth2/access_token?appid={appId}&secret={appSecret}&code={code}&grant_type=authorization_code";
            var res = HttpHelper.Get(url);
            Tracer.Debug($"RequestAccessToken\nRequest:{url}\nResponse:{res}");
            return res;
        }


    }

    public enum AuthScopeType
    {
        /// <summary>
        /// 不弹出授权页面，直接跳转，只能获取用户openid
        /// </summary>
        snsapi_base,

        /// <summary>
        /// 弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息
        /// </summary>
        snsapi_userinfo,
    }
}
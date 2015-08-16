using CS.Cryptography;
using CS.Diagnostics;
using NUnit.Framework;

namespace CS.WeChat.Tests.WebChat
{
    [TestFixture]
    public class JsApiTest
    {

        [Test]
        public void JsTicketTest()
        {
            for (int i = 0; i < 10; i++)
            {
                var ticket = JsApi.JsTicket;
                Tracer.Debug(ticket);
            }
        }


        [Test]
        public void SHA1Test()
        {
            Tracer.Debug(Sha1.Encrypt("jsapi_ticket=sM4AOVdWfPE4DxkXGEs8VMCPGGVi4C3VM0P37wVUCFvkVAy_90u5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcHKP7qg&noncestr=Wm3WZYTPz0wzccnW&timestamp=1414587457&url=http://mp.weixin.qq.com?params=value"));

            Assert.Equals(
                Sha1.Encrypt(
                    "jsapi_ticket=sM4AOVdWfPE4DxkXGEs8VMCPGGVi4C3VM0P37wVUCFvkVAy_90u5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcHKP7qg&noncestr=Wm3WZYTPz0wzccnW&timestamp=1414587457&url=http://mp.weixin.qq.com?params=value"), "0f9de62fce790f9a083d5c99e95740ceb90c27ed");
        }

        [Test]
        public void GetDefaultAppIdTest()
        {
            Tracer.Debug(AppContainer.GetDefaultAppId());
        }

    }
}
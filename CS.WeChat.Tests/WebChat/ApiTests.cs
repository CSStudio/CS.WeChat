using System.Runtime.ConstrainedExecution;
using System.Threading;
using CS.Diagnostics;
using NUnit.Framework;

namespace CS.WeChat.Tests.WebChat
{

    [TestFixture]
    public class ApiTests
    {
        private const string appId = "wxd171c38d55f771b8";
        private const string appSecret = "8d4b1b0d104ba65dafb6809499afd772";

        [SetUp]
        public void Setup()
        {
            Api.Init(appId, appSecret);
        }

        [Test]
        public void RequestTokenTest()
        {
            var result = Api.RequestToken(appId, appSecret);
            Tracer.Debug(result);
        }

        [Test]
        public void GetTokenTest()
        {
            var json = Api.GetToken(appId, appSecret);
            Tracer.Debug($"Token:{json.AccessToken}");
            Tracer.Debug($"Expires:{json.ExpiresIn}");
        }

        [Test]
        public void ApiTokenTest()
        {
            var token = Api.AccessToken;
            Tracer.Debug(token);
        }

        [Test]
        public void ApiTokenCacheTest()
        {
            for (int i = 0; i < 100; i++)
            {
                var token = Api.AccessToken;
                Tracer.Debug(token);
                Thread.Sleep(100);
            }
        }

    }
}
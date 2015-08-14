using CS.Diagnostics;
using NUnit.Framework;

namespace CS.WeChat.Tests.WebChat
{
    [TestFixture]
    public class ApiHelperTest
    {
        [Test]
        public void RandomStringTest()
        {
            for (int i = 0; i < 50; i++)
            {
                Tracer.Debug(ApiHelper.GetRandomString());
            }
        }

    }
}
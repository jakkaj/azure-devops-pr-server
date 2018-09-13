using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR.Helpers;

namespace PRServer.Tests
{
    [TestClass]
    public class TestToken : TestBase
    {
        [TestMethod]
        public async Task TestGetTokens()
        {
            var appId = SecretOptions.Value.AppId;
            var secret = SecretOptions.Value.Password;
            var tenant = SecretOptions.Value.TenantId;

            var h = new SecurityHelpers();

            var token = await h.MakeTokenCredentials(appId, secret, tenant);

            Assert.IsFalse(string.IsNullOrWhiteSpace(token.Token));
        }
    }
}

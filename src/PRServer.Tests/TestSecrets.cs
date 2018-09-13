using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PRServer.Tests
{
    [TestClass]
    public class TestSecrets : TestBase
    {
        [TestMethod]
        public void EnsureSecretsPresent()
        {
            var secret = TestBase.SecretOptions;
            Assert.IsFalse(string.IsNullOrWhiteSpace(secret.Value.AppId));
            Assert.IsFalse(string.IsNullOrWhiteSpace(secret.Value.Password));
            Assert.IsFalse(string.IsNullOrWhiteSpace(secret.Value.TenantId));
            Assert.IsFalse(string.IsNullOrWhiteSpace(secret.Value.PAT));
            Assert.IsFalse(string.IsNullOrWhiteSpace(AppSettings.AzureDevOpsCollectionName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(AppSettings.Genre));
            Assert.IsFalse(string.IsNullOrWhiteSpace(AppSettings.Name));
        }
    }
}

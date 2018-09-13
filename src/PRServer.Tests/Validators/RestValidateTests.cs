using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR.Helpers.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace PRServer.Tests.Rest
{
    [TestClass]
    public class RestValidateTests : TestBase
    {
        [TestMethod]
        public async Task TestBadArmTemplates()
        {
            var tester = Services.GetService<IRestValidateCheck>();
            var builder = Services.GetService<ITemplateBuilder>();
            var secret = TestBase.SecretOptions;

            var sample = File.ReadAllText("ArmSamples\\TestNSG_badSubnet.json");


            var built = builder.Build("FullTemplate.json", sample);
            var result = await tester.CheckArm(built, secret.Value);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.error);
            Assert.IsTrue(result.error.details.Count > 0);

            Debug.WriteLine($"{result.error.details[0].code} - {result.error.details[0].message}");
            
        }

        [TestMethod]
        public async Task TestBadArmTemplates2()
        {
            var tester = Services.GetService<IRestValidateCheck>();
            var builder = Services.GetService<ITemplateBuilder>();
            var secret = TestBase.SecretOptions;

            var sample = File.ReadAllText("ArmSamples\\TestNSG_badpriority.json");


            var built = builder.Build("FullTemplate.json", sample);
            var result = await tester.CheckArm(built, secret.Value);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.error);
            Assert.IsTrue(result.error.details.Count > 0);

            Debug.WriteLine($"{result.error.details[0].code} - {result.error.details[0].message}");

        }


        [TestMethod]
        public async Task TestGoodArmTemplates()
        {
            var tester = Services.GetService<IRestValidateCheck>();
            var builder = Services.GetService<ITemplateBuilder>();
            var secret = TestBase.SecretOptions;

            var sample = File.ReadAllText("ArmSamples\\TestNSG_good.json");


            var built = builder.Build("FullTemplate.json", sample);
            var result = await tester.CheckArm(built, secret.Value);



            Assert.IsNotNull(result);

            Assert.IsTrue(result.properties.provisioningState == "Succeeded");
        }
    }
}

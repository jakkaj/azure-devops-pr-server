using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PR.Helpers.Contract;

namespace PRServer.Tests.Validators
{
    [TestClass]
    public class JsonValidationTests : TestBase
    {
        [TestMethod]
        public async Task BasicJsonValidatorGoodAndBad()
        {
            var validator = Resolve<IBasicJsonValidator>();
            var runner = Resolve<IValidatorRunner>();
            var sample = File.ReadAllText("ArmSamples\\BrokenJson.json");
            var sample2 = File.ReadAllText("ArmSamples\\TestNSG_good.json");

            var l = new List<(string,string)>();
            l.Add(("Sample1", sample));
            l.Add(("Sample2", sample2));

            var result = await runner.ValidateAll(l, validator);

            Assert.IsTrue(result.Failed);

            Assert.IsTrue(result.ValidatedFiles[0].Failed);
            Assert.IsFalse(result.ValidatedFiles[1].Failed);


        }

        [TestMethod]
        public async Task BasicJsonValidatorBad()
        {
            var validator = Resolve<IBasicJsonValidator>();
            var runner = Resolve<IValidatorRunner>();
            var sample = File.ReadAllText("ArmSamples\\BrokenJson.json");

            var l = new List<(string, string)>();
            l.Add(("Sample1", sample));

            var result = await runner.ValidateAll(l, validator);

            Assert.IsTrue(result.Failed);

            Assert.IsTrue(result.ValidatedFiles[0].Failed);
        }

        [TestMethod]
        public async Task BasicJsonValidatorGood()
        {
            var validator = Resolve<IBasicJsonValidator>();
            var runner = Resolve<IValidatorRunner>();
            var sample = File.ReadAllText("ArmSamples\\TestNSG_good.json");

            var l = new List<(string, string)>();
            l.Add(("Sample1", sample));

            var result = await runner.ValidateAll(l, validator);

            Assert.IsFalse(result.Failed);

            Assert.IsFalse(result.ValidatedFiles[0].Failed);
        }

        [TestMethod]
        public void ValidateJsonReadFail()
        {
            var sample = File.ReadAllText("ArmSamples\\BrokenJson.json");
            try
            {
                var parsed = JObject.Parse(sample);

                var result = parsed;
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
    }
}

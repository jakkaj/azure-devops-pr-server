using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PR.Helpers.Models;
using Microsoft.Extensions.DependencyInjection;
using PR.Helpers.Contract;

namespace PRServer.Tests.VSTS
{
    [TestClass]
    public class PRProcessor : TestBase
    {
        [TestMethod]
        public async Task TestNewPR()
        {
            var data = File.ReadAllText("SerialisedObjects\\req4.json");

            var obj = JsonConvert.DeserializeObject<VstsRequest>(data);

            var prProcessor = Resolve<IPRProcessor>();

            await prProcessor.HandleARM_PR(obj);
        }
    }
}

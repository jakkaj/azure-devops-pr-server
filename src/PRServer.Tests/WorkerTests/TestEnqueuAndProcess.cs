using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR.Helpers.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Newtonsoft.Json;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PRServer.Tests.WorkerTests
{
    [TestClass]
    public class TestEnqueuAndProcess : TestBase
    {
        /// <summary>
        /// These are horrible tests used during dev to dev the queuing stuff. 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestEnqueueVstsTask()
        {
            var sbc = Services.GetService<QueuedHostedService>();
            var queue = Services.GetService<IBackgroundTaskQueue>();
            var prProcessor = Resolve<IPRProcessor>();

            var data = File.ReadAllText("SerialisedObjects\\req3.json");

            var obj = JsonConvert.DeserializeObject<VstsRequest>(data);


            Debug.WriteLine("Starting Queue");
            await sbc.StartAsync(new CancellationToken());

            Debug.WriteLine("Started Queue");
            queue.QueueBackgroundWorkItem(async token =>
            {
                await prProcessor.HandleARM_PR(obj);
            });
            
            await Task.Delay(20000);
        }

        [TestMethod]
        public async Task TestEnqueueAndGetsProcessed()
        {
            var sbc = Services.GetService<QueuedHostedService>();
            var queue = Services.GetService<IBackgroundTaskQueue>();
            Debug.WriteLine("Starting Queue");
            await sbc.StartAsync(new CancellationToken());

            Debug.WriteLine("Started Queue");
            queue.QueueBackgroundWorkItem(async token =>
            {
                var guid = Guid.NewGuid().ToString();

                for (int delayLoop = 0; delayLoop < 3; delayLoop++)
                {
                    Debug.WriteLine(
                        $"Queued Background Task {guid} is running. {delayLoop}/3");
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                }

                Debug.WriteLine(
                    $"Queued Background Task {guid} is complete. 3/3");
            });

            await Task.Delay(10000);

        }
    }
}

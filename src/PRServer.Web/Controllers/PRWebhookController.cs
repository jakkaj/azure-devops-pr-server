using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PR.Helpers.Contract;
using PR.Helpers.Models;
using PR.Helpers.Workers;

namespace PRServer.Web.Controllers
{
    [Route("api/prcreate")]
    [ApiController]
    public class PRWebhookController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IPRProcessor _prProcessor;
        private readonly IBackgroundTaskQueue _taskQueue;

        public PRWebhookController(ILogService logService, IPRProcessor prProcessor, IBackgroundTaskQueue taskQueue)
        {
            _logService = logService;
            _prProcessor = prProcessor;
            _taskQueue = taskQueue;
        }

        [HttpPost]
        [Route("nsgCheck")]
        public async Task<IActionResult> CreateNSGCheckPR([FromBody] VstsRequest req)
        {
            _taskQueue.QueueBackgroundWorkItem(async token => { await _prProcessor.HandleARM_PR(req); });
            

            var ser = JsonConvert.SerializeObject(req);
            System.IO.File.WriteAllText("C:\\Temp\\ser\\req6.json", ser);
            return Ok();
            //using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            //{
            //    var s=  await reader.ReadToEndAsync();
            //    Debug.WriteLine(s);
            //    return s;
            //}
        }
    }
}
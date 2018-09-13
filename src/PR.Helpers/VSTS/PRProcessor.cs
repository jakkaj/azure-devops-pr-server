using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.VSTS
{
    public class PRProcessor : IPRProcessor
    {
        private readonly ILogService _logService;
        private readonly IVstsHelper _vstsHelper;
        private readonly IValidatorRunner _validatorRunner;
        private readonly IBasicJsonValidator _basicJsonValidator;
        private readonly IARMValidator _armValidator;

        public PRProcessor(ILogService logService, IVstsHelper vstsHelper,
            IValidatorRunner validatorRunner,
            IBasicJsonValidator basicJsonValidator,
            IARMValidator armValidator)
        {
            _logService = logService;
            _vstsHelper = vstsHelper;
            _validatorRunner = validatorRunner;
            _basicJsonValidator = basicJsonValidator;
            _armValidator = armValidator;
        }

        public async Task HandleARM_PR(VstsRequest req)
        {
            _logService.TrackEventObject(PR.Helpers.Contract.Constants.Events.INBOUND_PR, "input_pr_json", req);

            try
            {
                var files = await _vstsHelper.GetFilesinPR(req);
                _logService.TrackTrace("Files loaded", "count", files.Count.ToString());

                await _vstsHelper.SetToPending(req);

                var basicResult = await _validatorRunner.ValidateAll(files, _basicJsonValidator);

                if (basicResult.Failed)
                {
                    await _vstsHelper.ReportBackToPullRequest(req, basicResult);
                    return;
                }

                var armResult = await _validatorRunner.ValidateAll(files, _armValidator);
                
                await _vstsHelper.ReportBackToPullRequest(req, armResult);
            }
            catch (Exception ex)
            {
                _logService.TrackException(ex);
                return;
            }
            
        }
    }
}

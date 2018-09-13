using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.Validators
{
    public class ValidatorRunner : IValidatorRunner
    {
        private readonly ILogService _logService;

        public ValidatorRunner(ILogService logService)
        {
            _logService = logService;
        }
        public async Task<AzureValidationResult> ValidateAll(List<(string path, string contents)> files, ISimpleValidator validator)
        {
            var validationResult = new AzureValidationResult
            {
                ValidationType = validator.ValidatorName
            };

            foreach (var file in files)
            {
                var fileResult = await validator.Validate(file.path, file.contents);
                validationResult.ValidatedFiles.Add(fileResult);
                
                var valDict = new Dictionary<string,string>();
                valDict.Add("FileName", file.path);
                valDict.Add("Failed", fileResult.Failed.ToString());
                valDict.Add("ValidationType", validationResult.ValidationType);
                valDict.Add("Message", fileResult.Message);

                if (fileResult.Failed)
                {
                    _logService.TrackEvent("JsonValidationFailed", valDict);
                }
                else
                {
                    _logService.TrackEvent("JsonValidationSucceeded", valDict);
                }
            }

            validationResult.Failed = validationResult.ValidatedFiles.Any(_ => _.Failed);

            return validationResult;
        }
    }
}

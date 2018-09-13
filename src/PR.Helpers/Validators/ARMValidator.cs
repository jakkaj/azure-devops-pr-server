using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.Validators
{
    public class ARMValidator : IARMValidator
    {
        private readonly IRestValidateCheck _restChecker;
        private readonly ITemplateBuilder _templateBuilder;
        private readonly Secrets _secrets;
        public ARMValidator(IRestValidateCheck restChecker, ITemplateBuilder templateBuilder, IOptions<Secrets> options)
        {
            _restChecker = restChecker;
            _templateBuilder = templateBuilder;
            _secrets = options.Value;
        }
        public async Task<AzureValidatedFile> Validate(string path, string contents)
        {
            var valid = new AzureValidatedFile
            {
                FilePath = path,
                Message = ""
            };

            try
            {
                //load all the subtemplates and process them through
                var parsed = JObject.Parse(contents);

                var builtTemplate = _templateBuilder.Build("FullTemplate.json", contents);

                var result = await _restChecker.CheckArm(builtTemplate, _secrets);

                if (result.error != null)
                {
                    valid.Failed = true;

                    var mString = "";

                    if (result?.error?.details != null && result?.error?.details.Count > 0)
                    {
                        mString = string.Join(" \r\n ", result.error.details.Select(_ => _.message + "\r\n").ToList());
                    }
                    else
                    {
                        mString = result.error.message;
                    }

                    valid.Message += $"{mString}\r\n";
                }
            }
            catch (Exception ex)
            {
                valid.Failed = true;
                valid.Message = $"JSON is not valid or process failed -> {ex.Message}";
                Debug.WriteLine(ex.Message);
            }

            return valid;
        }

        public string ValidatorName => "ARMValidator";
    }
}


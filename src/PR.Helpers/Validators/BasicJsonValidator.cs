using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.Validators
{
    public class BasicJsonValidator : ISimpleValidator, IBasicJsonValidator
    {
        public string ValidatorName => "BasicJsonValidator";

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<AzureValidatedFile> Validate(string path, string contents)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var valid = new AzureValidatedFile
            {
                FilePath = path,
                Message = $"JSON is valid"
            };

            try
            {
                var parsed = JObject.Parse(contents);
            }
            catch (JsonReaderException ex)
            {
                valid.Failed = true;
                valid.Message = $"JSON is not valid -> {ex.Message}";
                Debug.WriteLine(ex.Message);
            }

            return valid;
        }
    }
}

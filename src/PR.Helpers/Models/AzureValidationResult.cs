using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PR.Helpers.Models
{
    public class AzureValidationResult
    {
        public AzureValidationResult()
        {
            ValidatedFiles = new List<AzureValidatedFile>();
        }
        public bool Failed { get; set; }
        public List<AzureValidatedFile> ValidatedFiles { get; set; }
        public string ValidationType { get; set; }
    }

    public class AzureValidatedFile
    {
        public bool Failed { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
    }
}

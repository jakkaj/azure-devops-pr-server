using System.Collections.Generic;
using System.Threading.Tasks;
using PR.Helpers.Models;

namespace PR.Helpers.Contract
{
    public interface IValidatorRunner
    {
        Task<AzureValidationResult> ValidateAll(List<(string path, string contents)> files, ISimpleValidator validator);
    }
}
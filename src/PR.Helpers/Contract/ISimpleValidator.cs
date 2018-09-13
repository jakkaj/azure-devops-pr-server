using System.Collections.Generic;
using System.Threading.Tasks;
using PR.Helpers.Models;

namespace PR.Helpers.Contract
{
    public interface ISimpleValidator
    {
        Task<AzureValidatedFile> Validate(string path, string contents);
        string ValidatorName { get; }
    }
}
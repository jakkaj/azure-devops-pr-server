using System.Threading.Tasks;
using PR.Helpers.Models;

namespace PR.Helpers.Contract
{
    public interface IRestValidateCheck
    {
        Task<AzValidateResponse> CheckArm(string template, Secrets secret);
    }
}
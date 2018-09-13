using System.Threading.Tasks;
using PR.Helpers.Models;

namespace PR.Helpers.Contract
{
    public interface ISecurityHelpers
    {
        Task<SecurityTokenResponse> 
            MakeTokenCredentials(string appId, string secret, string tenantId);
    }
}
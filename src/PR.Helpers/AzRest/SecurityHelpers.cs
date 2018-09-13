using System;

using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers
{
    public class SecurityHelpers : ISecurityHelpers
    {
        public async Task<SecurityTokenResponse> 
            MakeTokenCredentials(string appId, string secret, string tenantId)
        {
            var authority = $"https://login.windows.net/{tenantId}";
            var resource = "https://management.azure.com/";

            var authContext = new AuthenticationContext(authority);
            var credential = new ClientCredential(appId, secret);

            var count = 0;
            Exception exception = null;

            while (count < 5)
            {
                try
                {
                    var authResult = await authContext.AcquireTokenAsync(resource, credential);
                    return new SecurityTokenResponse {Token = authResult.AccessToken};

                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                count++;
            }

            return new SecurityTokenResponse {Ex = exception};
        }
    }

}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.AzRest
{
    public class RestValidateCheck : IRestValidateCheck
    {
        private ISecurityHelpers _securityHelpers;
        private Settings _settings;
        public RestValidateCheck(ISecurityHelpers secHelper, IOptions<Settings> _settingsOpts)
        {
            _securityHelpers = secHelper;
            _settings = _settingsOpts.Value;
        }

        public async Task<AzValidateResponse> CheckArm(string template, Secrets secret)
        {
            var token = await _securityHelpers.MakeTokenCredentials(secret.AppId, secret.Password, secret.TenantId);
            //sometimes it fails for some reason, transient. 
            if (token.Ex != null)
            {
                var res = new AzValidateResponse
                {
                    Exception = token.Ex
                };

                return res;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://management.azure.com/");
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                var request = new HttpRequestMessage(HttpMethod.Post, 
                        $"subscriptions/{_settings.AzureSubId}/resourcegroups/{_settings.ResourceGroupName}/providers/Microsoft.Resources/deployments/508fb979-84ad-442e-aafe-1c6a19d4da5a/validate?api-version=2017-05-10");

                request.Content = new StringContent(template,
                    Encoding.UTF8,
                    "application/json");//CONTENT-TYPE header
                var result = await client.SendAsync(request);
                
                var body = await result.Content.ReadAsStringAsync();

                try
                {
                    var deserialised = JsonConvert.DeserializeObject<AzValidateResponse>(body);
                    return deserialised;
                }
                catch (Exception ex)
                {
                    var res = new AzValidateResponse
                    {
                        Exception = ex
                    };

                    return res;
                }
            }
        }
    }
}

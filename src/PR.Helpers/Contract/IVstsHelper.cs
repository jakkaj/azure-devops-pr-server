using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PR.Helpers.Models;

namespace PR.Helpers.Contract
{
    public interface IVstsHelper
    {
        Task<List<(string path, string contents)>> GetFilesinPR(VstsRequest req);
        Task ReportBackToPullRequest(VstsRequest req, AzureValidationResult result);
        Task SetToPending(VstsRequest req);
    }
}
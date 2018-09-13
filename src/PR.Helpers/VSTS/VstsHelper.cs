using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.VSTS
{
    public class VstsHelper : IVstsHelper
    {
        private readonly IOptions<Secrets> _secrets;
        private readonly IOptions<Settings> _settings;
        private readonly ILogService _logService;

        public VstsHelper(IOptions<Secrets> secrets, IOptions<Settings> settings, ILogService logService)
        {
            _secrets = secrets;
            _settings = settings;
            _logService = logService;
        }

        VssConnection _getConnection(VstsRequest req)
        {
            return new VssConnection(new Uri(_settings.Value.AzureDevOpsCollectionName),
                new VssBasicCredential(string.Empty, _secrets.Value.PAT));
        }

        GitHttpClient _getGitClient(VstsRequest req)
        {
            return _getConnection(req).GetClient<GitHttpClient>();
        }

        async Task _createComment(string message, GitPullRequest pr, GitHttpClient gitClient, string repoId)
        {
            var comment = new Comment();
            comment.Content = message;

            var cThread = new GitPullRequestCommentThread
            {
                Comments = new List<Comment>()
            };

            cThread.Comments.Add(comment);

            await gitClient.CreateThreadAsync(cThread, repoId, pr.PullRequestId);
        }



        public async Task SetToPending(VstsRequest req)
        {
            var gitClient = _getGitClient(req);
            var prStatus = new GitPullRequestStatus();
            prStatus.Context = new GitStatusContext { Genre = _settings.Value.Genre, Name = _settings.Value.Name };
            prStatus.State = GitStatusState.Pending;
            var repoId = req.resource.repository.id;
            var gRepoId = new Guid(repoId);

            var projectId = req.resourceContainers.project.id;

            var pr = await gitClient.GetPullRequestAsync(repoId, req.resource.pullRequestId);

            var its = await gitClient.GetPullRequestIterationsAsync(repoId, pr.PullRequestId);

            var it = its.Last();
            prStatus.IterationId = it.Id;

            await gitClient.CreatePullRequestIterationStatusAsync(prStatus, new Guid(repoId), pr.PullRequestId, it.Id.Value);
        }

        public async Task ReportBackToPullRequest(VstsRequest req, AzureValidationResult result)
        {
            var gitClient = _getGitClient(req);

            var repoId = req.resource.repository.id;
            var gRepoId = new Guid(repoId);

            var projectId = req.resourceContainers.project.id;

            var pr = await gitClient.GetPullRequestAsync(repoId, req.resource.pullRequestId);

            var its = await gitClient.GetPullRequestIterationsAsync(repoId, pr.PullRequestId);

            var it = its.Last();

            var prStatus = new GitPullRequestStatus();
            prStatus.Context = new GitStatusContext { Genre = _settings.Value.Genre, Name = _settings.Value.Name };
            prStatus.IterationId = it.Id;

            if (result.Failed)
            {
                prStatus.State = GitStatusState.Failed;
                prStatus.Description = "Failed ARM Template Checks";

                var failed = result.ValidatedFiles.FirstOrDefault(_ => _.Failed);

                var message = "";

                if (failed == null)
                {
                    _logService.TrackException(new Exception("Failed happened but not failed item found!"));
                    message = $"## ARM Validation Failed on Update {it.Id} \r\n Failed checks on Update {it.Id}";
                    //should not be able to do this:/
                }
                else
                {
                    message =
                        $"## ARM Validation Failed on Update {it.Id}\r\n**File:** {failed.FilePath}\r\n**Message:** {failed.Message}";
                }

                await _createComment(message, pr, gitClient, repoId);


            }
            else
            {
                var message = $"## ARM Validation Passed on Update {it.Id}\r\n Scanned {result.ValidatedFiles.Count} files.";
                await _createComment(message, pr, gitClient, repoId);
                prStatus.State = GitStatusState.Succeeded;
                prStatus.Description = "ARM Template Checks Pass";
            }

            await gitClient.CreatePullRequestIterationStatusAsync(prStatus, new Guid(repoId), pr.PullRequestId, it.Id.Value);

        }


        public async Task<List<(string path, string contents)>> GetFilesinPR(VstsRequest req)
        {
            //var connection = new VssConnection(new Uri(_secrets.Value.VSTSCollection),
            //    new VssBasicCredential(string.Empty, _secrets.Value.PAT));

            // var c = connection.AuthenticatedIdentity;

            var gitClient = _getGitClient(req);

            var repoId = req.resource.repository.id;
            var projectId = req.resourceContainers.project.id;
            var branchRef = req.resource.sourceRefName.Replace("refs/heads/", "");


            var versionDesc = new GitVersionDescriptor
            {
                VersionType = GitVersionType.Branch,
                Version = branchRef
            };
            try
            {
                var items = await gitClient.GetItemsAsync(projectId, repoId,
                    "/", VersionControlRecursionType.Full, versionDescriptor: versionDesc);

                var result = new List<(string path, string contents)>();

                foreach (var i in items.Where(_ => !_.IsFolder))
                {
                    if (i.Path.ToLower().IndexOf("gitignore", StringComparison.Ordinal) != -1)
                    {
                        continue;
                    }
                    var content = await gitClient.GetItemAsync(repoId,
                        i.Path, includeContent: true, versionDescriptor: versionDesc);

                    result.Add((i.Path, content.Content.ToString()));

                }

                return result;
            }

            catch (Exception ex)
            {
                _logService.TrackException(ex);
            }

            return new List<(string path, string contents)>();
        }
    }
}

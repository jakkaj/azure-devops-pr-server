using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PR.Helpers.Contract;
using PR.Helpers.Models;
using Microsoft.Extensions.DependencyInjection;
namespace PRServer.Tests.VSTS
{
    [TestClass]
    public class GitTests : TestBase
    {
        [TestMethod]
        public async Task CreatePRStatus()
        {
            var data = File.ReadAllText("SerialisedObjects\\req3.json");

            var obj = JsonConvert.DeserializeObject<VstsRequest>(data);


            //  VssConnection connection = new VssConnection(new Uri(collectionUri), new VssBasicCredential(string.Empty, pat));

            var collectionId = AppSettings.AzureDevOpsCollectionName;

            
            var connection = new VssConnection(new Uri(collectionId),
                new VssBasicCredential(string.Empty, SecretOptions.Value.PAT));
            
            var gitClient = connection.GetClient<GitHttpClient>();

            var repoId = obj.resource.repository.id;
            var gRepoId = new Guid(repoId);
            var pr = await gitClient.GetPullRequestAsync(obj.resource.repository.id, obj.resource.pullRequestId);
            var its = await gitClient.GetPullRequestIterationsAsync(repoId, pr.PullRequestId);

            var it = its.Last();
            var prStatus = new GitPullRequestStatus();
            prStatus.Context = new GitStatusContext {Genre = "testing", Name = "prtester"};
            prStatus.State = GitStatusState.Succeeded;
            prStatus.Description = "All seems good dude6.";
            prStatus.IterationId = it.Id;

            var comment = new Comment();
            comment.Content = "This was done okay cool yeah *bold* **something** \r\n # test \r\n omething";

            var cThread = new GitPullRequestCommentThread
            {
                Comments = new List<Comment>()
            };

            cThread.Comments.Add(comment);

            await gitClient.CreateThreadAsync(cThread, repoId, pr.PullRequestId);
            
            await gitClient.CreatePullRequestIterationStatusAsync(prStatus, new Guid(repoId), pr.PullRequestId, it.Id.Value);

        }

        [TestMethod]
        public async Task TestPullClean()
        {
            var data = File.ReadAllText("SerialisedObjects\\vsts_req.json");

            var obj = JsonConvert.DeserializeObject<VstsRequest>(data);

            var vsts = Services.GetService<IVstsHelper>();

            var files = await vsts.GetFilesinPR(obj);

            Assert.IsNotNull(files);

            Assert.IsTrue(files.Count > 0);

        }

        [TestMethod]
        public async Task TestPull()
        {
            var data = File.ReadAllText("SerialisedObjects\\vsts_req.json");

            var obj = JsonConvert.DeserializeObject<VstsRequest>(data);
          
            var collectionId = AppSettings.AzureDevOpsCollectionName;

            var connection = new VssConnection(new Uri(collectionId), 
                new VssBasicCredential(string.Empty, SecretOptions.Value.PAT));

           var gitClient = connection.GetClient<GitHttpClient>();

            var repoId = obj.resource.repository.id;

            var pr = await gitClient.GetPullRequestAsync(obj.resource.repository.id, obj.resource.pullRequestId);
            
            var versionDesc = new GitVersionDescriptor
            {
                VersionType = GitVersionType.Branch,
                Version = obj.resource.sourceRefName.Replace("refs/heads/", "")
            };

            var items = await gitClient.GetItemsAsync(obj.resourceContainers.project.id, obj.resource.repository.id,
                "/", VersionControlRecursionType.Full,versionDescriptor: versionDesc);

            foreach (var i in items.Where(_ => !_.IsFolder))
            {
                var content = await gitClient.GetItemAsync(obj.resource.repository.id, i.Path, includeContent: true,
                    versionDescriptor: versionDesc);
            }
        }
    }
}

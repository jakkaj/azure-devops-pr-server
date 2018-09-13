using System;
using System.Collections.Generic;
using System.Text;

namespace PR.Helpers.Models
{

    public class Message
    {
        public string text { get; set; }
        public string html { get; set; }
        public string markdown { get; set; }
    }

    public class DetailedMessage
    {
        public string text { get; set; }
        public string html { get; set; }
        public string markdown { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public string visibility { get; set; }
    }

    public class Repository
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public string defaultBranch { get; set; }
        public string remoteUrl { get; set; }
    }

    public class CreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
    }

    public class LastMergeSourceCommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class LastMergeTargetCommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class LastMergeCommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class Reviewer
    {
        public object reviewerUrl { get; set; }
        public int vote { get; set; }
        public string displayName { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public bool isContainer { get; set; }
    }

    public class Commit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class Web
    {
        public string href { get; set; }
    }

    public class Statuses
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Web web { get; set; }
        public Statuses statuses { get; set; }
    }

    public class Resource
    {
        public Repository repository { get; set; }
        public int pullRequestId { get; set; }
        public string status { get; set; }
        public CreatedBy createdBy { get; set; }
        public DateTime creationDate { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string sourceRefName { get; set; }
        public string targetRefName { get; set; }
        public string mergeStatus { get; set; }
        public string mergeId { get; set; }
        public LastMergeSourceCommit lastMergeSourceCommit { get; set; }
        public LastMergeTargetCommit lastMergeTargetCommit { get; set; }
        public LastMergeCommit lastMergeCommit { get; set; }
        public List<Reviewer> reviewers { get; set; }
        public List<Commit> commits { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
    }

    public class Collection
    {
        public string id { get; set; }
    }

    public class Account
    {
        public string id { get; set; }
    }

    public class Project2
    {
        public string id { get; set; }
    }

    public class ResourceContainers
    {
        public Collection collection { get; set; }
        public Account account { get; set; }
        public Project2 project { get; set; }
    }

    public class VstsRequest
    {
        public string subscriptionId { get; set; }
        public int notificationId { get; set; }
        public string id { get; set; }
        public string eventType { get; set; }
        public string publisherId { get; set; }
        public Message message { get; set; }
        public DetailedMessage detailedMessage { get; set; }
        public Resource resource { get; set; }
        public string resourceVersion { get; set; }
        public ResourceContainers resourceContainers { get; set; }
        public DateTime createdDate { get; set; }

    }
}

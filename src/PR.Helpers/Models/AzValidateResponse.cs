using System;
using System.Collections.Generic;
using System.Text;

namespace PR.Helpers.Models
{
    public class Detail
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<object> details { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<Detail> details { get; set; }
    }

    public class AzValidateResponse
    {
        public Exception Exception { get; set; }
        public Error error { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Properties properties { get; set; }
        public class NetworkSecurityGroupName
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class Location
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class Parameters
        {
            public NetworkSecurityGroupName networkSecurityGroupName { get; set; }
            public Location location { get; set; }
        }

        public class ResourceType
        {
            public string resourceType { get; set; }
            public List<string> locations { get; set; }
        }

        public class Provider
        {
            public string @namespace { get; set; }
            public List<ResourceType> resourceTypes { get; set; }
        }

        public class Properties3
        {
            public string access { get; set; }
            public int priority { get; set; }
            public string direction { get; set; }
            public string protocol { get; set; }
            public string sourcePortRange { get; set; }
            public List<object> sourcePortRanges { get; set; }
            public string destinationPortRange { get; set; }
            public List<string> destinationPortRanges { get; set; }
            public string sourceAddressPrefix { get; set; }
            public object sourceAddressPrefixes { get; set; }
            public string destinationAddressPrefix { get; set; }
            public List<string> destinationAddressPrefixes { get; set; }
        }

        public class SecurityRule
        {
            public string name { get; set; }
            public Properties3 properties { get; set; }
        }

        public class Properties2
        {
            public List<SecurityRule> securityRules { get; set; }
        }

        public class ValidatedResource
        {
            public string apiVersion { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string location { get; set; }
            public Properties2 properties { get; set; }
        }

        public class Properties
        {
            public string templateHash { get; set; }
            public Parameters parameters { get; set; }
            public string mode { get; set; }
            public string provisioningState { get; set; }
            public DateTime timestamp { get; set; }
            public string duration { get; set; }
            public string correlationId { get; set; }
            public List<Provider> providers { get; set; }
            public List<object> dependencies { get; set; }
            public List<ValidatedResource> validatedResources { get; set; }
        }
    }
}

# Azure DevOps Pull Request Server

Azure DevOps has the capability to call out to external services and have them send approvals back before a pull request is considered valid. 

This [ASP.NET Core](https://www.microsoft.com/net/download) 2 based sample accepts the call out, grabs the files from the pull request and validates them against the Azure ARM template validation end point. 

This type of real-world check validates that the ARM template will deploy - including checking for detailed correctness. Something obscure such as overlapping networks in NSG templates would not be picked up by straight up JSON schema validation. 

A [node.js sample](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-pr-status-server?view=vsts) is available. 

This version is written in .NET Core and uses the official Azure DevOps SDK from Nuget. 

## Video

Afer you've run the setup instructures below, check out the [Video Demo](https://www.youtube.com/watch?v=QAPGEzNb9dg) on YouTube.

## Background Reading

- [Service Hooks](https://docs.microsoft.com/en-us/azure/devops/service-hooks/services/webhooks?view=vsts)
- [Pull Request Status](https://docs.microsoft.com/en-us/azure/devops/repos/git/pull-request-status?view=vsts)
- [Branch Policies](https://docs.microsoft.com/en-us/azure/devops/repos/git/branch-policies?view=vsts)

## Getting Started

You'll need some access and accounts and stuff. 


Prepare a notepad document with the following:

    dotnet user-secrets set Secrets:AppId 
    dotnet user-secrets set Secrets:Password 
    dotnet user-secrets set Secrets:TenantId 
    dotnet user-secrets set Secrets:PAT 


- I'm using Visual Studio 2017 - but you could just as easily use Visual Studio Code for this
- An Azure Subscription
- The Azure CLI Installed (for creating things)
- A [Service Principal](https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli?view=azure-cli-latest)
    - Populate AppId, Password and TenantId from the SP creation process in to that notepad doc
- An Azure DevOps Account
- An Azure DevOps [Personal Access Token](https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=vsts) (PAT) on the right account
    - Fill this in the notepad doc with this as well
- A resource group in your tenant
    - In Visual Studio, populate the group name in `appsettings.json` for `PRServer.Tests` and `PRServer.Web`
    - Same for your azure subscription Id (you can see this when you log in to the AZ CLI)
- Populate your Azure DevOps collection name in the two settings files also - I'm using the old VSTS version of this name - you can probably do this too. 

Your file should look something like this:

```
    dotnet user-secrets set Secrets:AppId 32kjlsf9w3 

    dotnet user-secrets set Secrets:Password  0sdf0-9sdfjsdf 

    dotnet user-secrets set Secrets:TenantId 9834099dsf 

    dotnet user-secrets set Secrets:PAT sdf0s83ljks 
```

In Visual Studion
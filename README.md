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
- Download [ngrok](https://ngrok.com/) 

Your file should look something like this:

```
    dotnet user-secrets set Secrets:AppId 32kjlsf9w3 

    dotnet user-secrets set Secrets:Password  0sdf0-9sdfjsdf 

    dotnet user-secrets set Secrets:TenantId 9834099dsf 

    dotnet user-secrets set Secrets:PAT sdf0s83ljks 
```

Open the .sln file in Visual Studio (or open the folder if in Visual Studio Code) and rebuild the project. 

### Apply the secrets

Navigate to the PRServer.Tests folder from a terminal and paste in the settings you collected in notepad. These are the secrets and should not be pasted in to your settings files!

You can edit the settings in Azure by following [this blog post](https://blogs.msdn.microsoft.com/waws/2018/06/12/asp-net-core-settings-for-azure-app-service/). 

## Take it from the top

The high level flow is:

- Run the Web Site in Visual Studio
- Create an ngrok tunnel to it 
- Create a service hook for Pull Request Created and Updated
- Enforce branch policies
- Create a PR
- Fail the checks
- Edit the PR
- Pass the checks

[This article](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-pr-status-server?view=vsts) does a better job of explaining the process than mine!


### Run the Web Site in Visual Studio

Self explanitory. Run it, once the browser pops, grab the port number. 

### Create an ngrok tunnel to it

ngrok is cool! It allows you to go from internet to your local machine without having to mess with firewalls and things. 

ngrok will need some parameters to help .NET Core accept the requests from the weird outside host:

Substitute your port and run:

```
ngrok http 54934 -host-header="localhost:54934"
```

In the screen that pops you'll see some urls. Copy the https one.

![ngrok](https://user-images.githubusercontent.com/5225782/45523399-78c38d00-b80c-11e8-9683-3a7a30743f83.PNG)

Add to this URL `/api/prcreate/nsgCheck` which is the controller endpoint on the web app in Visual Studio. 

e.g.
```
https://89c5fcf4.ngrok.io/api/prcreate/nsgCheck
```

### Create a service hook for Pull Request Created and Updated

In VS, create a break point in the controller so you can check it's working. 

Navigate to your Project Settings (in the project, bottom left, not outside where you can see all projects at the tenant level). Click "Service hooks".

Add a new hook and select "Web Hooks". Click next. 

This screen you can customise all the branches and things - no need to make any changes now, but you will probably need to in your final setup. 

Select "Pull request created". 

![new web chook](https://user-images.githubusercontent.com/5225782/45523547-2c2c8180-b80d-11e8-8de3-7e296bc24b1f.PNG).

Click next. 

Enter the Url you constructed from ngrok with your api enpoint and click "Test". 

Your breakpoint should be hit! Just let it through. 

Now repeat the process to add a web hook for "Pull request updated". 

### Enforce branch policies

Now to create rules for the Pull Request. 

Navigate to "Repos\Branches" in the Azure DevOps menu. 

Use master, or create a branch to play with. 

Hover over master and click the "..." ellipsis and select "Branch Policies". 

To turn on enforced pull requests select "Require a minimum number of reviewers", set min to 1 for testing (so you can approve yourself) and also select "Allow users to approve their own changes.". This is not a great production setting, but it's good for testing. Normally you'd not allow users to approve themselves! Although there is a use case for enforced PR when policy checks such as this are in place so they can approve themselves but must pass policy first... cool!



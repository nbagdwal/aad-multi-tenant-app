---
services: active-directory
platforms: dotnet
endpoint: Microsoft identity platform
page_type: sample
author: Narendra Bagdwal
client: ASP.NET Core Web App
languages:
- csharp
products:
- Azure Active Directory
- aspnet-core
- azure-cosmos-db
- dotnet
- CSharp

description: "This sample shows you how to use the Microsoft Azure Active Directory, Azure Cosmos DB service to create a mutli tenant app and store, access data from an ASP.NET Core MVC application."
---

# Overview

Developers can choose to configure their app to be either single-tenant or multi-tenant during app registration in the [Azure portal](https://portal.azure.com).

- `Single-tenant` apps are only available in the tenant they were registered in, also known as their home tenant.
- `Multi-tenant` apps are available to users in both their home tenant and other tenants where they are provisioned. Apps that allow users to sign-in using their personal accounts that they use to sign into services like Xbox and Skype are also multi-tenant apps.

For more information about apps and tenancy, see [Tenancy in Azure Active Directory](https://docs.microsoft.com/azure/active-directory/develop/single-and-multi-tenant-apps)


# Multi tenant application development using Azure Active Directroy, ASP.NET Core MVC and Azure Cosmos DB

This sample shows you how to use the Microsoft Azure active directory, Cosmos DB service to create mulit tenant application and store, access data from Azure Cosmos DB using ASP.NET Core MVC application hosted on Azure App Service or running locally in your computer.

# Configure the sample to use your Azure AD tenant

As a first step you'll need to:

1. Sign in to the [National cloud Azure portal](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-national-cloud#app-registration-endpoints) using either a work or school account or a personal Microsoft account.
1. If your account is present in more than one Azure AD tenant, select your profile at the top right corner in the menu on top of the page, and then **switch directory**.
   Change your portal session to the desired Azure AD tenant.

## Register an App in Azure Active Directory

1. Navigate to the Microsoft identity platform for developers [App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) page.
1. Click **New registration** on top.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `MultiTenantApp`.
   - Change **Supported account types** to **Accounts in any organizational directory**.
     > Note that there are more than one redirect URIs used in this sample. You'll need to add them from the **Authentication** tab later after the app has been created successfully.
1. Click on the **Register** button in bottom to create the application.
1. In the app's registration screen, find the **Application (client) ID** value and record it for use later. You'll need it to configure the configuration file(s) later in your code.
1. In the app's registration screen, click on the **Authentication** blade in the left.
   - In the Redirect URIs section, select **Web** in the drop down and enter the following redirect URIs.
           - `https://localhost:44307/`
           - `https://localhost:44307/signin-oidc`
        - In the **Advanced settings** section, set **Logout URL** to `https://localhost:44307/signout-oidc`.
        - In the **Advanced settings** | **Implicit grant** section, check the **ID tokens** option as the [AspNetCore security middleware](https://github.com/aspnet/AspNetCore/tree/master/src/Security) used in the sample uses the [Implicit grant flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-implicit-grant-flow) by default to get the user info right after sign-in.

1. Click the **Save** button on top to save the changes.
1. In the app's registration screen, click on the **Certificates & secrets** blade in the left to open the page where we can generate secrets and upload certificates.
1. In the **Client secrets** section, click on **New client secret**:
   - Type a key description (for instance `app secret`),
   - Select one of the available key durations (**In 1 year**, **In 2 years**, or **Never Expires**) as per your security concerns.
   - The generated key value will be displayed when you click the **Add** button. Copy the generated value for use in the steps later.
   - You'll need this key later in your code's configuration files. This key value will not be displayed again, and is not retrievable by any other means, so make sure to note it from the Azure portal before navigating to any other screen or blade.
1. In the app's registration screen, click on the **API permissions** blade in the left to open the page where we add access to the Apis that your application needs.
   - Click the **Add a permission** button and then,
   - In the **Delegated permissions** section, select the **User.Read.All** in the list. Use the search box if necessary.
   - Click on the **Add permissions** button in the bottom.

## Running this sample in Visual Studio

To run this sample:

> Pre-requisites: Install .NET 5  by following the instructions at [.NET and C# - Get Started in 10 Minutes](https://www.microsoft.com/net/core). .

Ideally, you would want to have two Azure AD tenants so you can test all the aspects of this multi-tenant sample. For more information on how to get an Azure AD tenant, see [How to get an Azure AD tenant](https://azure.microsoft.com/documentation/articles/active-directory-howto-tenant/).

1. Before you can run this sample, you must have the following prerequisites:
    - Visual Studio 2019 (or higher).
    - An active Azure Cosmos account or the [Azure Cosmos DB Emulator](https://docs.microsoft.com/azure/cosmos-db/local-emulator) - If you don't have an account, refer to the [Create a database account](https://docs.microsoft.com/azure/cosmos-db/create-sql-api-dotnet#create-an-azure-cosmos-db-account) article

2. Clone this repository using Git for Windows (http://www.git-scm.com/), or download the zip file.

3. From Visual Studio, open the [Notes.csproj](./src/Notes.csproj).

4. In Visual Studio Build menu, select **Build Solution** (or Press F6). 

5. Retrieve the URI and PRIMARY KEY (or SECONDARY KEY) values from the Keys blade of your Azure Cosmos account in the Azure portal. For more information on obtaining endpoint & keys for your Azure Cosmos account refer to [View, copy, and regenerate access keys and passwords](https://docs.microsoft.com/azure/cosmos-db/secure-access-to-data#master-keys)  **if you are going to work with a real Azure Cosmos account**.

6. In the [appsettings.json](./src/appsettings.json) file, located in the project root, find **Account** and **Key** and replace the placeholder values with the values obtained for your account if you are going to work with a real Azure Cosmos account.

7. Configure the project to use registerd App.
    - Open the `appsettings.json` file
    - Find the app key `Instance` and replace the existing value with the corresponding [Azure AD endpoint](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-national-cloud#azure-ad-authentication-endpoints) for the national cloud you want to target.
    - Find the app key `ClientId` and replace the existing value with the application ID (clientId) of the `WebApp-MultiTenant-v2` application copied from the Azure portal.
    - Find the app key `TenantId` and replace the existing value with `organizations`.
    - Find the app key `Domain` and replace the existing value with your Azure AD tenant name.
    - Find the app key `ClientSecret` and replace the existing value with the key you saved during the creation of the `WebApp-MultiTenant-v2` app, in the Azure portal.

8. You can now run and debug the application locally by pressing **F5** in Visual Studio.

### Deploy this sample to Azure in Visual Studio

1. In Visual Studio Solution Explorer, right-click on the project name and select **Publish...**

2. Using the *Pick a publish target* dialog, select **App Service**

3. Either select an existing App Service, or follow the prompts to create new one. Note: If you choose to create a new one, the App Name chosen must be globally unique. 

4. Once you have selected the App Service, click **Publish**

5. After a short time, Visual Studio will complete the deployment.

For additional ways to deploy this web application to Azure, please refer to the [Deploy ASP.NET Core apps to Azure App Service](https://docs.microsoft.com/aspnet/core/host-and-deploy/azure-apps/?view=aspnetcore-2.2) article which includes information on using Azure Pipelines, CLI, and many more. 


## Running this sample from the .NET Core command line

1. Before you can run this sample, you must have the following prerequisites:
    - Install .NET 5  by following the instructions at [.NET and C# - Get Started in 10 Minutes](https://dotnet.microsoft.com/download)
    - An active Azure Cosmos account or the [Azure Cosmos DB Emulator](https://docs.microsoft.com/azure/cosmos-db/local-emulator) - If you don't have an account, refer to the [Create a database account](https://docs.microsoft.com/azure/cosmos-db/create-sql-api-dotnet#create-an-azure-cosmos-db-account) article

2. Clone this repository using your Git command line, or download the zip file.

3. Go to the location of the [Notes.csproj](./src/Notes.csproj) in your command line prompt.

4. Run `dotnet build` to restore packages and build the project.

5. Retrieve the URI and PRIMARY KEY (or SECONDARY KEY) values from the Keys blade of your Azure Cosmos account in the Azure portal. For more information on obtaining endpoint & keys for your Azure Cosmos account refer to [View, copy, and regenerate access keys and passwords](https://docs.microsoft.com/azure/cosmos-db/secure-access-to-data#master-keys) **if you are going to work with a real Azure Cosmos account**.

6. In the [appsettings.json](./src/appsettings.json) file, located in the project root, find **Account** and **Key** and replace the placeholder values with the values obtained for your account if you are going to work with a real Azure Cosmos account.

7. Configure the project to use registerd App.
    - Open the `appsettings.json` file
    - Find the app key `Instance` and replace the existing value with the corresponding [Azure AD endpoint](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-national-cloud#azure-ad-authentication-endpoints) for the national cloud you want to target.
    - Find the app key `ClientId` and replace the existing value with the application ID (clientId) of the `WebApp-MultiTenant-v2` application copied from the Azure portal.
    - Find the app key `TenantId` and replace the existing value with `organizations`.
    - Find the app key `Domain` and replace the existing value with your Azure AD tenant name.
    - Find the app key `ClientSecret` and replace the existing value with the key you saved during the creation of the `WebApp-MultiTenant-v2` app, in the Azure portal.

8. You can now run and debug the application locally by running `dotnet run` and browsing the Url provided by the .NET Core command line.

### Deploy this sample to Azure with Visual Studio Code

1. Install the [Visual Studio Code extension](https://code.visualstudio.com/tutorials/app-service-extension/getting-started#_install-the-extension).

2. Sign in to your Azure account.

3. Click on the [Deploy](https://code.visualstudio.com/tutorials/app-service-extension/deploy-app) button on the Azure App Service extension.

4. Select the `src` folder to deploy. 

5. Select your Azure subscription and whether you want to select an existing Web App or create a new one. Note: If you choose to create a new one, the App Name chosen must be globally unique. 

6. After a short time, Visual Studio Code will complete the deployment.

For additional ways to deploy this web application to Azure, please refer to [Deploy to Azure using App Service](https://code.visualstudio.com/tutorials/app-service-extension/getting-started) or [Deploy to Azure using the Azure CLI](https://code.visualstudio.com/tutorials/nodejs-deployment/getting-started).


## About the code
The code included in this sample is intended to get you going with a simple multi teanat Azure Active Directory app using C#, ASP.NET Core MVC that connects to Azure Cosmos DB. It is not intended to be a set of best practices on how to build scalable enterprise grade web applications. This is beyond the scope of this quick start sample. 

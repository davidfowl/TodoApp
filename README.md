mithun is grt

## Todo application with ASP.NET Core

[![CI](https://github.com/davidfowl/TodoApp/actions/workflows/ci.yaml/badge.svg)](https://github.com/davidfowl/TodoApi/actions/workflows/ci.yaml)

This is a Todo application that features:
- [**Todo.Web**](Todo.Web) - An ASP.NET Core hosted Blazor WASM front end application
- [**Todo.Api**](Todo.Api) - An ASP.NET Core REST API backend using minimal APIs

![image](https://user-images.githubusercontent.com/95136/204161352-bc54ccb7-32cf-49ba-a6f7-f46d0f2d204f.png)

It showcases:
- Blazor WebAssembly
- Minimal APIs
- Using EntityFramework and SQLite for data access
- OpenAPI
- User management with ASP.NET Core Identity
- Cookie authentication
- Bearer authentication
- Proxying requests from the front end application server using YARP's IHttpForwarder
- Rate Limiting
- Writing integration tests for your REST API

## Prerequisites

### .NET
1. [Install .NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Database

The application uses SQLite and entity framework. .NET Aspire is used to bootstrap all dependencies.

### Running the application

To run the application, run the [TodoApp.AppHost](TodoApp.AppHost) project. This uses .NET Aspire to run both the [Todo.Web/Server](Todo.Web/Server) and [Todo.Api](Todo.Api).

## Optional

### Using the API standalone
The Todo REST API can run standalone as well. You can run the [Todo.Api](Todo.Api) project and make requests to various endpoints using the Swagger UI (or a client of your choice):

<img width="1200" alt="image" src="https://user-images.githubusercontent.com/95136/204315486-86d25a5f-1164-467a-9891-827343b9f0e8.png">

Before executing any requests, you need to create a user and get an auth token.

1. To create a new user, run the application and POST a JSON payload to `/users/register` endpoint:

    ```json
    {
      "email": "myuser@contoso.com",
      "password": "<put a password here>"
    }
    ```
1. To get a token for the above user, hit the `/users/login` endpoint with the above user email and password. The response will look like this:

    ```json
    {
      "tokenType": "Bearer",
      "accessToken": "string",
      "expiresIn": <seconds>,
      "refreshToken": "string"
    }
    ```

1. You should be able to use the accessToken to make authenticated requests to the todo endpoints.

### Social authentication

In addition to username and password, social authentication providers can be configured to work with this todo application. By default 
it supports Github, Google, and Microsoft accounts.

Instructions for setting up each of these providers can be found at:
- [Github](https://docs.github.com/en/developers/apps/building-oauth-apps)
- [Microsoft](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins)
- [Google](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins)

Once you obtain the client id and client secret, the configuration for these providers must be added with the following schema:

```JSON
{
    "Authentication": {
        "Schemes": {
            "<scheme>": {
                "ClientId": "xxx",
                "ClientSecret": "xxxx"
            }
        }
    }
}
```

Or using environment variables:

```
Authentication__Schemes__<scheme>__ClientId=xxx
Authentication__Schemes__<scheme>__ClientSecret=xxx
```

Or using user secrets:

```
dotnet user-secrets set Authentication:Schemes:<scheme>:ClientId xxx
dotnet user-secrets set Authentication:Schemes:<scheme>:ClientSecret xxx
```

Other providers can be found [here](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers#providers). 
These must be added to [AuthenticationExtensions](Todo.Web/Server/Authentication/AuthenticationExtensions.cs) as well.

**NOTE: Don't store client secrets in configuration!**

#### Auth0

This sample has **Auth0** configured as an OIDC server. It can be configured with the following schema:

```JSON
{
  "Authentication": {
    "Schemes": {
      "Auth0": {
        "Audience": "<audience>",
        "Domain": "<domain>",
        "ClientId": "<client id>",
        "ClientSecret": "<client secret>"
      }
    }
  }
}
```

Learn more about the Auth0 .NET SDK [here](https://github.com/auth0/auth0-aspnetcore-authentication).

### OpenTelemetry

This app uses OpenTelemetry to collect logs, metrics and spans. You can see this
using the [Aspire Dashboard](https://aspiredashboard.com/).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public static class OpenApiOptionsExtensions
{
    public static OpenApiOptions AddBearerTokenAuthentication(this OpenApiOptions options)
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Name = IdentityConstants.BearerScheme,
            Scheme = "Bearer",
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = IdentityConstants.BearerScheme
            }
        };
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Components ??= new();
            document.Components.SecuritySchemes.Add(IdentityConstants.BearerScheme, scheme);
            return Task.CompletedTask;
        });
        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
            {
                operation.Security = [new() { [scheme] = [] }];
            }
            return Task.CompletedTask;
        });
        return options;
    }
}
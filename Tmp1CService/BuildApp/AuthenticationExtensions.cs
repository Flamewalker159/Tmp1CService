using Microsoft.AspNetCore.Authentication;
using Tmp1CService.Security;

namespace Tmp1CService.BuildApp;

public static class AuthenticationExtensions
{
    public static void AddApiKeyAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", null);
    }
}
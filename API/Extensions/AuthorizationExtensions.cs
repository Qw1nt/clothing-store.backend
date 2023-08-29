using System.Security.Claims;
using Domain.Common.Configurations;

namespace API.Extensions;

/// <summary>
/// Методы-расширения авторизации
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Политика авторизации
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorizationWithPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityConfiguration.Policy.Admin,
                policy =>
                {
                    policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, IdentityConfiguration.Roles.Admin);
                });
            options.AddPolicy(IdentityConfiguration.Policy.Manager,
                policy =>
                {
                    policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, IdentityConfiguration.Roles.Admin,
                        IdentityConfiguration.Roles.Manager);
                });
        });

        return services;
    }
}
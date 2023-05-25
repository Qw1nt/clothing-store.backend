using ClothingStore.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ClothingStore.API.Extensions;

/// <summary>
/// Методы-расширения аутентификации
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Добавляет JWT аутентификацию
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AuthenticationConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = configuration.SymmetricSecurityKey(),

                ClockSkew = TimeSpan.Zero,
            };
        });
        
        return services;
    }
}
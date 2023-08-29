using Application.Common.Contracts;
using Domain.Common.Configurations;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.FileSaveService;
using Infrastructure.Services.HashSalt;
using Infrastructure.Services.UpdateProductService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var authenticationConfig = configuration.GetSection(AuthenticationConfiguration.SectionKey).Get<AuthenticationConfiguration>()!;
        services.AddJwtAuthentication(authenticationConfig);
        services.AddSingleton(authenticationConfig);
        
        services.AddScoped<IUpdateProductService, UpdateProductService>();
        
        services.AddScoped<IHashSaltService, ComputeHashSaltService>();
        services.AddScoped<JwtGenerationService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        services.AddScoped<IFileSaveService, FileSaveService>(_ => new FileSaveService
        {
            WebRootDirectoryPath = environment.WebRootPath
        });

        return services;
    }
}
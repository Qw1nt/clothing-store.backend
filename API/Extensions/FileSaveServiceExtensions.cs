using Application.Common.Contracts;
using Infrastructure.Services.FileSaveService;

namespace API.Extensions;

/// <summary>
/// 
/// </summary>
public static class FileSaveServiceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddFileSaveService<T>(this IServiceCollection services, WebApplicationBuilder builder)
        where T : FilesSaveServiceBase, IFileSaveService, new()
    {
        services.AddScoped<IFileSaveService, T>(_ => new T
        {
            WebRootDirectoryPath = builder.Environment.WebRootPath,
        });
        
        return services;
    }
}
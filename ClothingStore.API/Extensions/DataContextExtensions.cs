using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.API.Extensions;

/// <summary>
/// Методы-расширения для DataContext
/// </summary>
public static class DataContextExtensions
{
    /// <summary>
    /// Подключить PostgreSQL
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConnectionStringConfiguration.Database));
        });

        return services;
    }
}
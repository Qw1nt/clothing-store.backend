using Application.Common.Contracts;
using Domain;
using Domain.Common.Configurations;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

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
        services.AddDbContext<IApplicationDataContext, ApplicationDataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConnectionStringConfiguration.Database));
        });

        return services;
    }
}
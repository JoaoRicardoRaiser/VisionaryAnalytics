using Microsoft.EntityFrameworkCore;
using VisionaryAnalytics.Api.Infrastructure.Database;

namespace VisionaryAnalytics.Api.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDatabase(configuration);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VisionaryAnalyticsApiDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        return services;
    }
}

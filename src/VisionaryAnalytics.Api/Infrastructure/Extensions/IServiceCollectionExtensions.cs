using MongoDB.Driver;
using VisionaryAnalytics.Api.Domain.Interfaces;
using VisionaryAnalytics.Api.Infrastructure.Database.Repositories;

namespace VisionaryAnalytics.Api.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDatabase(configuration);
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new MongoClient(configuration.GetConnectionString("MongoDb")));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        return services;
    }
}

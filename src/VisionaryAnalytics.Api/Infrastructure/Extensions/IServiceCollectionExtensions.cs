using MongoDB.Driver;
using Raisersoft.EasyRabbit.Extensions;
using VisionaryAnalytics.Api.Application.Dtos;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Infrastructure.Database.Repositories;

namespace VisionaryAnalytics.Api.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddRabbitMq();
        return services;
    }

    private static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddEasyRabbitMq();

        services.AddPublisher<VideoReceivedEventDto>("VideoReceived");

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new MongoClient(configuration.GetConnectionString("MongoDb")));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVideoRepository, VideoRepository>();
        return services;
    }
}

using System.Reflection;
using VisionaryAnalytics.Api.Application.Interfaces.Services;
using VisionaryAnalytics.Api.Application.Services;

namespace VisionaryAnalytics.Api.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddProfiles();
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVideoService, VideoService>();

        return services;
    }
}

using Raisersoft.EasyRabbit.Interfaces;
using VisionaryAnalytics.Worker.Application.Dtos;
using VisionaryAnalytics.Worker.Application.Interfaces.Services;
using VisionaryAnalytics.Worker.Application.MessageHandlers;
using VisionaryAnalytics.Worker.Application.Services;

namespace VisionaryAnalytics.Worker.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {
        services.AddServices();
        services.AddMessageHandlers();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVideoService, VideoService>();

        return services;
    }

    private static IServiceCollection AddMessageHandlers(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler<VideoReceivedEventDto>, VideoReceivedMessageHandler>();

        return services;
    }
}

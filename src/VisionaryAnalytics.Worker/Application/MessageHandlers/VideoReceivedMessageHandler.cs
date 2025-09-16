using MongoDB.Bson;
using Raisersoft.EasyRabbit.Interfaces;
using VisionaryAnalytics.Worker.Application.Dtos;
using VisionaryAnalytics.Worker.Application.Interfaces.Services;

namespace VisionaryAnalytics.Worker.Application.MessageHandlers;

public class VideoReceivedMessageHandler(IVideoService service) : IMessageHandler<VideoReceivedEventDto>
{
    public async Task HandleAsync(VideoReceivedEventDto message)
        => await service.ProcessVideo(new ObjectId(message.Id));
}

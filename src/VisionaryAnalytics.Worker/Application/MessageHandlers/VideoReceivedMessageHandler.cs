using MongoDB.Bson;
using Raisersoft.EasyRabbit.Interfaces;
using VisionaryAnalytics.Worker.Application.Dtos;
using VisionaryAnalytics.Worker.Application.Interfaces.Services;

namespace VisionaryAnalytics.Worker.Application.MessageHandlers;

public class VideoReceivedMessageHandler : IMessageHandler<VideoReceivedEventDto>
{
    private readonly IVideoService _service;

    public VideoReceivedMessageHandler(IVideoService service)
    {
        _service = service;
    }

    public async Task HandleAsync(VideoReceivedEventDto message)
        => await _service.ProcessVideo(new ObjectId(message.Id));
}

using AutoMapper;
using VisionaryAnalytics.Api.Application.Dtos;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Application.Profiles;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<IFormFile, Video>()
            .ForMember(dst => dst.Name, opts => opts.MapFrom(src => src.FileName))
            .ForMember(dst => dst.LengthInMb, opts => opts.MapFrom(src => (src.Length / 1024) / 1024));

        CreateMap<Video, VideoReceivedEventDto>();
    }
}

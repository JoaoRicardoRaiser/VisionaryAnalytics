using AutoMapper;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Application.Profiles;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<IFormFile, Video>()
            .ForMember(src => src.Name, opts => opts.MapFrom(x => x.FileName));
    }
}

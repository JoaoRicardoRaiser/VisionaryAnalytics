using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Infrastructure.Database.EntityConfiguration;

public class VideoEntityTypeConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Status)
            .HasConversion<string>();
    }
}

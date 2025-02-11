using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SikiSokoChatApp.Domain.Entities;

namespace SikiSokoChatApp.Infrastructure.Persistence.EntityConfigurations;

public class MediaContentConfiguration : IEntityTypeConfiguration<MediaContent>
{
    public void Configure(EntityTypeBuilder<MediaContent> builder)
    {
        builder.ToTable("media_contents");

        builder.Property(x => x.MessageId).HasColumnName("message_id");
        builder.Property(x => x.FileName).HasColumnName("file_name")
            .HasMaxLength(255);
        builder.Property(x => x.ContentType).HasColumnName("content_type")
            .HasMaxLength(100);
        builder.Property(x => x.StoragePath).HasColumnName("storage_path")
            .HasMaxLength(1000);

        builder.HasOne(x => x.Message)
            .WithOne(x => x.MediaContent)
            .HasForeignKey<MediaContent>(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
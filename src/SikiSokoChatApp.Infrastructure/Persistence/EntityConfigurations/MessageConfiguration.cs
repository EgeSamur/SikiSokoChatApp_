using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SikiSokoChatApp.Domain.Entities;

namespace SikiSokoChatApp.Infrastructure.Persistence.EntityConfigurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.Property(x => x.ConversationId)
            .HasColumnName("conversation_id");

        builder.Property(x => x.SenderId)
            .HasColumnName("sender_id");

        builder.Property(x => x.MessageContent)
            .HasColumnName("message_content")
            .IsRequired(false);

        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasConversion<int>();

        // İlişkiler
        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MediaContent)
            .WithOne(x => x.Message)
            .HasForeignKey<MediaContent>(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);
      
    }
}

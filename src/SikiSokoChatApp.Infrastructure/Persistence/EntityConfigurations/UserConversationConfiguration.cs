using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SikiSokoChatApp.Domain.Entities;

namespace SikiSokoChatApp.Infrastructure.Persistence.EntityConfigurations;

public class UserConversationConfiguration : IEntityTypeConfiguration<UserConversation>
{
    public void Configure(EntityTypeBuilder<UserConversation> builder)
    {
        builder.ToTable("user_conversations");

        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.ConversationId).HasColumnName("conversation_id");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.Participants)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.ConversationId })
            .IsUnique();
    }
}

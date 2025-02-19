using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SikiSokoChatApp.Domain.Entities;

namespace SikiSokoChatApp.Infrastructure.Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(x => x.Username).HasColumnName("username").HasMaxLength(100);
        builder.Property(x => x.Fcm).HasColumnName("fcm").HasMaxLength(250);
        builder.Property(x => x.ContactCode).HasColumnName("contact_code")
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.ContactCode)
            .IsUnique();

        builder.HasMany(x => x.Conversations)
            .WithMany()
            .UsingEntity<UserConversation>();
    }
}

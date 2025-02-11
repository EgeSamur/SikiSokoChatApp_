using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SikiSokoChatApp.Domain.Entities;

namespace SikiSokoChatApp.Infrastructure.Persistence.EntityConfigurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("user_contacts");

        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.ContactUserId).HasColumnName("contact_user_id");
        builder.Property(x => x.Nickname).HasColumnName("nickname")
            .HasMaxLength(100);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Contacts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ContactUser)
            .WithMany()
            .HasForeignKey(x => x.ContactUserId)
            .OnDelete(DeleteBehavior.Cascade);

     
    }
}

using Microsoft.EntityFrameworkCore;
using SikiSokoChatApp.Domain.Entities;
using System.Reflection;

namespace SikiSokoChatApp.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<MediaContent> MediaContents { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserConversation> UserConversations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
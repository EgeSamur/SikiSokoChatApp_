using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SikiSokoChatApp.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    //public DbSet<User> Users { get; set; }
   
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
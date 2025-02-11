using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        //CityRepository = cityRepository;
      
    }

    private readonly ApplicationDbContext _context;
    
    //public ICityRepository CityRepository { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Rollback()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }
}

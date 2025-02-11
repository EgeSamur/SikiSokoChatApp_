using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Shared.Persistence.EfCore;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories;

// Infrastructure/Repositories/ContactRepository.cs
public class ContactRepository : RepositoryBase<Contact, ApplicationDbContext>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context)
    {
    }
}

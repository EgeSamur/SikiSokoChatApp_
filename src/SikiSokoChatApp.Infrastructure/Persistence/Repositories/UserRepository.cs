using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Shared.Persistence.EfCore;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories;

// Infrastructure/Repositories/UserRepository.cs
public class UserRepository : RepositoryBase<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}

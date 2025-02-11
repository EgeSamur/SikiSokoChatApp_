using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Shared.Persistence.EfCore;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories;

// Infrastructure/Repositories/UserConversationRepository.cs
public class UserConversationRepository : RepositoryBase<UserConversation, ApplicationDbContext>, IUserConversationRepository
{
    public UserConversationRepository(ApplicationDbContext context) : base(context)
    {
    }
}

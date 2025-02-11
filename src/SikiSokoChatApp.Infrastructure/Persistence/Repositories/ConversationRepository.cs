using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Shared.Persistence.EfCore;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories;

// Infrastructure/Repositories/ConversationRepository.cs
public class ConversationRepository : RepositoryBase<Conversation, ApplicationDbContext>, IConversationRepository
{
    public ConversationRepository(ApplicationDbContext context) : base(context)
    {
    }
}

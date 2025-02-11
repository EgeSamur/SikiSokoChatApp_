using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Shared.Persistence.EfCore;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories;

// Infrastructure/Repositories/MessageRepository.cs
public class MessageRepository : RepositoryBase<Message, ApplicationDbContext>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}

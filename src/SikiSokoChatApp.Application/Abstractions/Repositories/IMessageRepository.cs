using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Shared.Persistence.Abstraction;

namespace SikiSokoChatApp.Application.Abstractions.Repositories;

public interface IMessageRepository : IReadRepository<Message>, IWriteRepository<Message>
{
}


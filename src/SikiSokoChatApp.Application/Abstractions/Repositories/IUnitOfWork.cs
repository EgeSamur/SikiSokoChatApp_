using SikiSokoChatApp.Shared.Persistence.EfCore;
using System;

namespace SikiSokoChatApp.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IUserConversationRepository UserConversationRepository { get; }
    IMessageRepository MessageRepository { get; }
    IMediaContentRepository MediaContentRepository { get; }
    IContactRepository ContactRepository { get; }
    IConversationRepository ConversationRepository { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Rollback();
}


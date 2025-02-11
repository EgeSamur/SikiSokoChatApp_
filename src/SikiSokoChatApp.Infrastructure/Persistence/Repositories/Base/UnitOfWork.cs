using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IConversationRepository conversationRepository, IContactRepository contactRepository, IMediaContentRepository mediaContentRepository, IMessageRepository messageRepository, IUserConversationRepository userConversationRepository)
    {
        _context = context;
        UserRepository = userRepository;
        ConversationRepository = conversationRepository;
        ContactRepository = contactRepository;
        MediaContentRepository = mediaContentRepository;
        MessageRepository = messageRepository;
        UserConversationRepository = userConversationRepository;
    }

    private readonly ApplicationDbContext _context;

    public IUserRepository UserRepository { get; }
    public IConversationRepository ConversationRepository { get; }
    public IContactRepository ContactRepository { get; }
    public IMediaContentRepository MediaContentRepository { get; }
    public IMessageRepository MessageRepository { get; }
    public IUserConversationRepository UserConversationRepository { get; }


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



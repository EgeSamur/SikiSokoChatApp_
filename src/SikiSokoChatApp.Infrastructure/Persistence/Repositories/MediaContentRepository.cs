using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Infrastructure.Persistence.Contexts;
using SikiSokoChatApp.Shared.Persistence.EfCore;

namespace SikiSokoChatApp.Infrastructure.Persistence.Repositories;

// Infrastructure/Repositories/MediaContentRepository.cs
public class MediaContentRepository : RepositoryBase<MediaContent, ApplicationDbContext>, IMediaContentRepository
{
    public MediaContentRepository(ApplicationDbContext context) : base(context)
    {
    }
}

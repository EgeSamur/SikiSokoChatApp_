﻿using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Shared.Persistence.Abstraction;

namespace SikiSokoChatApp.Application.Abstractions.Repositories;

public interface IUserConversationRepository : IReadRepository<UserConversation>, IWriteRepository<UserConversation>
{
}


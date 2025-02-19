using SikiSokoChatApp.Shared.Domain.Entities;

namespace SikiSokoChatApp.Domain.Entities;

public class UserConversation : BaseEntity
{
    public int UserId { get; set; }
    public int ConversationId { get; set; }
    public User User { get; set; }
    public Conversation Conversation { get; set; }

}

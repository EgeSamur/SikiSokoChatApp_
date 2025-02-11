using SikiSokoChatApp.Shared.Domain.Entities;

namespace SikiSokoChatApp.Domain.Entities;

public class Conversation : BaseEntity
{
    public string? Name { get; set; }
    public bool IsGroup { get; set; }
    public List<UserConversation> Participants { get; set; }
    public List<Message> Messages { get; set; }


}

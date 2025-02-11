using SikiSokoChatApp.Shared.Domain.Entities;

namespace SikiSokoChatApp.Domain.Entities;

public class User : BaseEntity
{
    public string? Username { get; set; }
    public string Fcm { get; set; }
    public string ContactCode { get; set; }
    public List<Conversation> Conversations { get; set; }
    public List<Contact> Contacts { get; set; }

}

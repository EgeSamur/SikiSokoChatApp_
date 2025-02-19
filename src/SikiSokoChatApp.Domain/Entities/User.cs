using SikiSokoChatApp.Shared.Domain.Entities;

namespace SikiSokoChatApp.Domain.Entities;

public class User : BaseEntity
{
    public string? Username { get; set; }
    public string Fcm { get; set; }
    public string Password { get; set; } // hashlemeden direkt tutacağım. kınadığım şeyi yapıyorum :)
    public string ContactCode { get; set; }
    public List<Conversation> Conversations { get; set; }
}

using SikiSokoChatApp.Shared.Domain.Entities;

namespace SikiSokoChatApp.Domain.Entities;

public class Contact : BaseEntity
{
    public int UserId { get; set; }
    public int ContactUserId { get; set; }
    public string? Nickname { get; set; }
    public User User { get; set; }
    public User ContactUser { get; set; }
}

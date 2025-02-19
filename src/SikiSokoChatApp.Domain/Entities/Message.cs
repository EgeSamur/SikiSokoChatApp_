using SikiSokoChatApp.Domain.Enums;
using SikiSokoChatApp.Shared.Domain.Entities;

namespace SikiSokoChatApp.Domain.Entities;

public class Message : BaseEntity
{
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public int ReciverId { get; set; }
    public byte[]? MessageContent { get; set; } // text, pdf , image , video için tüm dataları base64 ile burada tutabiliriz.
    public string Content { get; set; }
    public MessageType Type { get; set; }
    public MediaContent? MediaContent { get; set; }
    public Conversation Conversation { get; set; }
    public User Sender { get; set; }
    public User Reciver { get; set; }
}


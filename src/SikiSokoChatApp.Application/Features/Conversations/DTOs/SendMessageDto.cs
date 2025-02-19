using SikiSokoChatApp.Domain.Enums;

namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class SendMessageDto
{
    public int SenderId { get; set; }   // Mesajı gönderen kişi
    public int ConversationId { get; set; }  // Hangi sohbete ait?
    public int? ReceiverId { get; set; }  // Özel mesajsa alıcı
    public string? Content { get; set; }  // Text mesaj içeriği
    public byte[]? MediaContent { get; set; }  // Eğer medya gönderiliyorsa (base64)
    public MessageType Type { get; set; }  // Mesaj tipi (Text, Image, Video, etc.)
}
public class MessageDto
{
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public string? SenderName { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; }
    public string? Base64MediaContent { get; set; } // Base64 olarak medya içeriği döndürülecek
    public MessageType Type { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    //sender Name , participants -> id'si , name'i fcmi,
}
public class GetMessagesFromConversationDto
{
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
}


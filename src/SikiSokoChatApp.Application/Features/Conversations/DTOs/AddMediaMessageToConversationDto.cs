namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class AddMediaMessageToConversationDto
{
    public int UserId { get; set; }   // Mesajı gönderen kişi
    public int ConversationId { get; set; }  // Hangi sohbete ait?
    public string FilePath{ get; set; }
}

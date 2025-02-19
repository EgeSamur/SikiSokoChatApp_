namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class AddPersonToConversationDto
{
    public string UserCode { get; set; }   // Mesajı gönderen kişi
    public int ConversationId { get; set; }  // Hangi sohbete ait?
}


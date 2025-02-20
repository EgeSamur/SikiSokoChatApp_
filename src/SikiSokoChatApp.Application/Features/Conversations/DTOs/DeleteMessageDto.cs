namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class DeleteMessageDto
{
    public int MessageSenderId { get; set; }   // Mesajı gönderen kişi
    public int ConversationId { get; set; }  // Hangi sohbete ait?
    public int MessageId { get; set; }
}

public class DeleteUserFromGroupDto
{
    public int UserId { get; set; } 
    public int ConversationId { get; set; }  
}

public class DeleteGroupDto
{
    public int ConversationId { get; set; }  
}

public class ClearChatDto
{
    public int ConversationId { get; set; }
}
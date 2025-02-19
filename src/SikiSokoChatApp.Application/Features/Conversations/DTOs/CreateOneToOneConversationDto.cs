namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class CreateOneToOneConversationDto
{
    public int CurrentUserId { get; set; }
    public string? ConversationName { get; set; }
    public string ContactUserCode { get; set; }
}

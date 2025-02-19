namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class ConversationDto
{
    public int ConversationId { get; set; }
    public List<int>? ContactUserIds { get; set; }
    public string? Name { get; set; }
    public string? LastMessage { get; set; }
}

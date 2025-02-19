namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class CreateGroupConversationDto
{
    public int CurrentUserId { get; set; }
    public string Name { get; set; }
    public List<string> ParticipantCodes { get; set; }
}

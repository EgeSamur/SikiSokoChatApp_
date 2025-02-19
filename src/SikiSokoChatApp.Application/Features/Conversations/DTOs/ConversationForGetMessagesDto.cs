namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class ConversationForGetMessagesDto
{
    public int ConversationId { get; set; }
    public List<ParticipantDto> Participants { get; set; }
    public List<MessageDto>? Messages { get; set; }
}
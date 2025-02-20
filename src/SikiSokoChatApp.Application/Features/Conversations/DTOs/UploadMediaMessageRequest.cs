using Microsoft.AspNetCore.Http;
using SikiSokoChatApp.Domain.Enums;

namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class UploadMediaMessageRequest
{
    public int SenderId { get; set; }
    public int ConversationId { get; set; }
    public List<IFormFile> Files { get; set; }
    public MessageType MessageType { get; set; }
}
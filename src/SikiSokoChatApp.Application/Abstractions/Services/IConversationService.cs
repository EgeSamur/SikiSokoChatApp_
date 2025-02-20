using Microsoft.AspNetCore.Mvc.Formatters;
using SikiSokoChatApp.Application.Features.Conversations.DTOs;
using SikiSokoChatApp.Application.Features.FileStorage.DTOs;
using SikiSokoChatApp.Application.Features.Users.DTOs;
using SikiSokoChatApp.Domain.Enums;
using SikiSokoChatApp.Shared.Utils.Responses;
using SikiSokoChatApp.Shared.Utils.Results.Abstract;

namespace SikiSokoChatApp.Application.Abstractions.Services;

public interface IConversationService
{
    Task<IResult> CreateOneToOneConversationAsync(CreateOneToOneConversationDto dto);
    Task<IResult> AddMessageToConversationAsync(SendMessageDto dto);
    Task<IDataResult<PaginatedResponse<ConversationDto>>> GetConvarsations(GetConversationsDto dto);
    Task<IDataResult<ConversationForGetMessagesDto>> GetMessagesFromConvarsation(GetMessagesFromConversationDto dto);
    Task<IResult> AddPersonToConversationAsync(AddPersonToConversationDto dto);
    Task<IDataResult<List<FilePathDto>>> UploadMediaMessage(UploadMediaMessageRequest dto);
    Task<IResult> DeleteAnyMessage(DeleteMessageDto dto);
    Task<IResult> DeleteConversation(DeleteGroupDto dto); 
    Task<IResult> ClearMessagesConversation(ClearChatDto dto); 
    Task<IResult> DeleteUserFromGroup(DeleteUserFromGroupDto dto);
}

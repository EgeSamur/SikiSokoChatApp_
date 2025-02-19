using AutoMapper;
using SikiSokoChatApp.Application.Features.Conversations.DTOs;
using SikiSokoChatApp.Shared.Utils.Pagination;
using SikiSokoChatApp.Shared.Utils.Responses;

namespace SikiSokoChatApp.Application.Features.Conversations.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //CreateMap<IPaginate<UserDto>, PaginatedResponse<UserDto>>();
            CreateMap<IPaginate<ConversationDto>, PaginatedResponse<ConversationDto>>();
            CreateMap<IPaginate<MessageDto>, PaginatedResponse<MessageDto>>();
            CreateMap<IPaginate<ConversationForGetMessagesDto>, PaginatedResponse<ConversationForGetMessagesDto>>();

        }
    }
}

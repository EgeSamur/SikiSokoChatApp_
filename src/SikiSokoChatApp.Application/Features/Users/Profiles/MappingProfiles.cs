using AutoMapper;
using SikiSokoChatApp.Application.Features.Users.DTOs;
using SikiSokoChatApp.Shared.Utils.Pagination;
using SikiSokoChatApp.Shared.Utils.Responses;

namespace SnifferApi.Application.Features.Users.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //CreateMap<IPaginate<UserDto>, PaginatedResponse<UserDto>>();
            CreateMap<IPaginate<UserDto>, PaginatedResponse<UserDto>>();

        }
    }
}

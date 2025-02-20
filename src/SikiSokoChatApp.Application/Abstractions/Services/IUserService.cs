using SikiSokoChatApp.Application.Features.Users.DTOs;
using SikiSokoChatApp.Shared.Utils.Responses;
using SikiSokoChatApp.Shared.Utils.Results.Abstract;

namespace SikiSokoChatApp.Application.Abstractions.Services;

public interface IUserService
{
    Task<IDataResult<UniqueCodeResponse>> CreateAsync(RegisterDto dto);
    Task<IDataResult<LoggedDto>> LoginAsync(LoginDto dto);
    Task<IResult> DeleteUserAsync(DeleteUserDto dto);
    Task<IDataResult<PaginatedResponse<UserDto>>> GetAllUsers();
}

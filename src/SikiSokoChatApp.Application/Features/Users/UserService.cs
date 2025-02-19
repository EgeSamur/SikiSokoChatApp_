using AutoMapper;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Common.Helpers;
using SikiSokoChatApp.Application.Features.Users.DTOs;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Types;
using SikiSokoChatApp.Shared.Utils.Responses;
using SikiSokoChatApp.Shared.Utils.Results.Abstract;
using SikiSokoChatApp.Shared.Utils.Results.Concrete;

namespace SikiSokoChatApp.Application.Features.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<UniqueCodeResponse>> CreateAsync(RegisterDto dto)
    {
        //fcm uniQue değilse zaten exception atacak.
        var code = RandomCodeGenerator.GenerateContactCode(15);
        var conversations = new List<Conversation>();

        var user = new User()
        {
            Fcm = dto.Fcm,
            Username = dto.UserName,
            Password = dto.Password,
            ContactCode = code,
            Conversations = conversations
        };
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        var codeResponse = new UniqueCodeResponse()
        {
            UserId = user.Id,
            Code = code
        };
        return new SuccessDataResult<UniqueCodeResponse>(codeResponse, "User Created.");
    }
    public async Task<IDataResult<LoggedDto>> LoginAsync(LoginDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(i => i.ContactCode == dto.Code);
        if (user == null)
            throw new NotFoundException("User does not exist.");
        if (dto.Code != user.ContactCode || dto.Password != user.Password)
            throw new BusinessException("Code or password is wrong.");

        var response = new LoggedDto()
        {
            UserId = user.Id,
            Username = user.Username,
            Fcm = user.Fcm,
            ContactCode= user.ContactCode
        };
        return new SuccessDataResult<LoggedDto>(response, "Logged.");
    }

}

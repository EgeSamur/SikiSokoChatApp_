using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Common.Helpers;
using SikiSokoChatApp.Application.Features.Conversations.DTOs;
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
    private readonly IFileStorageService _fileStorageService;


    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
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

    public async Task<IDataResult<PaginatedResponse<UserDto>>> GetAllUsers()
    {
        var users = await _unitOfWork.UserRepository.GetListWithProjectionAsync(isAll: true,
            selector: x => new UserDto()
            {
                Id = x.Id,
                Code = x.ContactCode,
                Fcm = x.Fcm,
                UserName = x.Username
            });
        var result = _mapper.Map<PaginatedResponse<UserDto>>(users);
        return new SuccessDataResult<PaginatedResponse<UserDto>>(result, "Conversations");
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
            ContactCode = user.ContactCode
        };
        return new SuccessDataResult<LoggedDto>(response, "Logged.");
    }
    public async Task<IResult> DeleteUserAsync(DeleteUserDto dto)
    {
        // Kullanıcıyı getir ve tüm ilişkili verileri yükle
        var userQueryable = _unitOfWork.UserRepository.Query()
            .Include(u => u.Conversations)
                .ThenInclude(c => c.Messages.Where(m => m.SenderId == dto.UserId))
                    .ThenInclude(m => m.MediaContent)
            .Include(u => u.Conversations)
                .ThenInclude(c => c.Participants);

        var user = await userQueryable.FirstOrDefaultAsync(u => u.Id == dto.UserId);


        if (user == null)
            throw new NotFoundException("User not found.");

        try
        {
            // 1. Kullanıcının gönderdiği tüm mesajlardaki medya dosyalarını sil
            foreach (var conversation in user.Conversations.ToList())
            {
                var userMessages = conversation.Messages
                    .Where(m => m.SenderId == dto.UserId && m.MediaContent != null)
                    .ToList();

                foreach (var message in userMessages)
                {
                    if (message.MediaContent != null)
                    {
                        await _fileStorageService.DeleteFileAsync(message.MediaContent.StoragePath);
                        await _unitOfWork.MediaContentRepository.HardDeleteAsync(message.MediaContent);
                    }
                }
            }

            // 2. Kullanıcının tüm mesajlarını sil
            var allUserMessages = user.Conversations
                .SelectMany(c => c.Messages)
                .Where(m => m.SenderId == dto.UserId)
                .ToList();

            foreach (var message in allUserMessages)
            {
                await _unitOfWork.MessageRepository.HardDeleteAsync(message);
            }

            // 3. Kullanıcının olduğu grupları kontrol et
            foreach (var conversation in user.Conversations.ToList())
            {

                // Grupta başka kullanıcılar varsa, sadece kullanıcının katılımcı kaydını sil
                if (conversation.Participants != null)
                {
                    var userParticipant = conversation.Participants.ToList()
                        .FirstOrDefault(p => p.UserId == dto.UserId);

                    if (userParticipant != null)
                    {
                        await _unitOfWork.UserConversationRepository.HardDeleteAsync(userParticipant);
                    }
                }

                else
                {
                    // Birebir sohbetse, tüm konuşmayı sil
                    if (conversation.Participants != null)
                    {
                        foreach (var participant in conversation.Participants.ToList())
                        {
                            await _unitOfWork.UserConversationRepository.HardDeleteAsync(participant);
                        }
                    }

                    await _unitOfWork.ConversationRepository.HardDeleteAsync(conversation);
                }
            }

            // 4. Son olarak kullanıcıyı sil
            await _unitOfWork.UserRepository.HardDeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("User and all related data deleted successfully.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while deleting user: {ex.Message}");
        }
    }
}

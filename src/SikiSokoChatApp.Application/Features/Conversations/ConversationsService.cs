using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Features.Conversations.DTOs;
using SikiSokoChatApp.Application.Features.FileStorage.DTOs;
using SikiSokoChatApp.Application.Features.Users.DTOs;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Domain.Enums;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Types;
using SikiSokoChatApp.Shared.Utils.Responses;
using SikiSokoChatApp.Shared.Utils.Results.Abstract;
using SikiSokoChatApp.Shared.Utils.Results.Concrete;

namespace SikiSokoChatApp.Application.Features;
public class ConversationsService : IConversationService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public ConversationsService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<IResult> CreateOneToOneConversationAsync(CreateOneToOneConversationDto dto)
    {
        try
        {
            var contactUser = await _unitOfWork.UserRepository.GetAsync(
            i => i.ContactCode == dto.ContactUserCode);
            if (contactUser == null)
                throw new NotFoundException("Contact user does not exist.");
            var existingConversation = await _unitOfWork.ConversationRepository.GetAsync(
             predicate: c => !c.IsGroup && c.Participants.Any(p => p.UserId == dto.CurrentUserId) &&
                            c.Participants.Any(p => p.UserId == contactUser.Id),
             include: query => query
                 .Include(c => c.Participants)
                 .ThenInclude(p => p.User)
            );
            //if (existingConversation != null)
            //    throw new Exception("You already have conversation with user.");
            var praticipants = new List<UserConversation>();
            var messages = new List<Message>();
            var conversation = new Conversation()
            {
                Name = dto.ConversationName,
                IsGroup = false,
                Participants = praticipants,
                Messages = messages
            };
            await _unitOfWork.ConversationRepository.AddAsync(conversation);
            await _unitOfWork.SaveChangesAsync();
          
            // Katılımcıları ekle
            conversation.Participants.Add(new UserConversation()
            {
                UserId = dto.CurrentUserId,
                ConversationId = conversation.Id
            });
            conversation.Participants.Add(new UserConversation()
            {
                UserId = contactUser.Id,
                ConversationId = conversation.Id
            });
            await _unitOfWork.ConversationRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("OkiDoki.");
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
       
    }

    public async Task<IDataResult<PaginatedResponse<ConversationDto>>> GetConvarsations(GetConversationsDto dto)
    {
        var currentUser = await _unitOfWork.UserRepository.GetAsync(i=>i.Id == dto.CurrentUserId);
        if(currentUser == null) 
            throw new NotFoundException("User not found.");
        var conversations =  await _unitOfWork.ConversationRepository.GetListWithProjectionAsync(
            predicate: c => c.Participants.Any(p => p.UserId == dto.CurrentUserId),
            selector: x => new ConversationDto()
            {
                ConversationId = x.Id,
                Name = x.Name,
                LastMessage = x.Messages
                .OrderByDescending(m => m.CreatedDate)
                .Select(m => m.Content)
                .FirstOrDefault(),
                ContactUserIds = x.Participants
                .Where(p => p.UserId != dto.CurrentUserId) // Kendi ID hariç diğer kullanıcıları al
                .Select(p => p.UserId)
                .ToList()
            });
        var result = _mapper.Map<PaginatedResponse<ConversationDto>>(conversations);
        return new SuccessDataResult<PaginatedResponse<ConversationDto>>(result, "Conversations");
    }

    public async Task<IResult> AddMessageToConversationAsync(SendMessageDto dto)
    {
        var sender = await _unitOfWork.UserRepository.GetAsync(u => u.Id == dto.SenderId);
        if (sender == null)
            throw new NotFoundException("Sender not found.");
        var conversation = await _unitOfWork.ConversationRepository.GetAsync(c => c.Id == dto.ConversationId,
            include: q => q.Include(c => c.Participants));
        if (conversation == null)
            throw new NotFoundException("Conversation not found.");
        //  Yeni mesajı oluştur
        var message = new Message
        {
            SenderId = dto.SenderId,
            ConversationId = dto.ConversationId,
            Content = dto.Content ?? string.Empty,
            MessageContent = null,
            Type = 0,
            CreatedDate = DateTime.UtcNow
        };
        await _unitOfWork.MessageRepository.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult("OkiDoki.");

    }

    public async Task<IDataResult<ConversationForGetMessagesDto>> GetMessagesFromConvarsation(GetMessagesFromConversationDto dto)
    {
        var conversation = await _unitOfWork.ConversationRepository.GetAsync(
            c => c.Id == dto.ConversationId,
            include: q => q.Include(c => c.Participants).ThenInclude(p => p.User));
        if (conversation == null)
            throw new NotFoundException("Conversation not found.");
        bool isParticipant = conversation.Participants.Any(p => p.UserId == dto.SenderId);
        if (!isParticipant)
            throw new UnauthorizedAccessException("User is not a participant of this conversation.");
        var messages = await _unitOfWork.MessageRepository.GetListWithProjectionAsync(
            isAll: true,
            selector: m => new MessageDto
            {
                MessageId = m.Id,
                SenderId = m.SenderId,
                SenderName = m.Sender.Username, // Sender'ın adını al
                Content = m.Content,
                Base64MediaContent = m.MessageContent != null ? Convert.ToBase64String(m.MessageContent) : null,
                Type = m.Type,
                CreatedDate = m.CreatedDate,
                MediaContent = m.MediaContent != null
                ? new MediaContentDto
                {
                    FileName = m.MediaContent.FileName,
                    ContentType = m.MediaContent.ContentType,
                    StoragePath = m.MediaContent.StoragePath
                }
                : null

            },
            predicate: m => m.ConversationId == dto.ConversationId, // Belirtilen konuşmaya ait mesajları getir
            orderBy: q => q.OrderBy(m => m.CreatedDate) // Mesajları tarih sırasına göre sırala
        );

        var conversationDto = new ConversationForGetMessagesDto
        {
            ConversationId = conversation.Id,
            Participants = conversation.Participants
                .Select(p => new ParticipantDto
                {
                    UserId = p.User.Id,
                    UserName = p.User.Username,
                    Fcm = p.User.Fcm
                }).ToList(),
            Messages = messages.Items.ToList()
        };
        return new SuccessDataResult<ConversationForGetMessagesDto>(conversationDto, "Messages");

    }

    public async Task<IResult> AddPersonToConversationAsync(AddPersonToConversationDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(u => u.ContactCode == dto.UserCode);
        if (user == null)
            throw new NotFoundException("User not found.");

        var conversation = await _unitOfWork.ConversationRepository.GetAsync(
            c => c.Id == dto.ConversationId,
            include: q => q.Include(c => c.Participants)
        );
        if (conversation == null)
            throw new NotFoundException("Conversation not found.");

        bool alreadyInConversation = conversation.Participants.Any(p => p.UserId == user.Id);
        if (alreadyInConversation)
            return new ErrorResult("User is already in this conversation.");
        var userConversation = new UserConversation
        {
            UserId = user.Id,
            ConversationId = dto.ConversationId
        };
        await _unitOfWork.UserConversationRepository.AddAsync(userConversation);
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResult("User added to conversation.");
    }

    public async Task<IDataResult<List<FilePathDto>>> UploadMediaMessage(UploadMediaMessageRequest dto)
    {
        var storagePaths = await _fileStorageService.SaveFileAsync(dto.Files, dto.SenderId, dto.MessageType);
        var response = new List<FilePathDto>();

        foreach (var (file, storagePath) in dto.Files.Zip(storagePaths))
        {
            var mediaContent = new MediaContent
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                StoragePath = storagePath
            };

            var message = new Message
            {
                SenderId = dto.SenderId,
                Content = "",
                ConversationId = dto.ConversationId,
                Type = dto.MessageType,
                MediaContent = mediaContent
            };

            await _unitOfWork.MessageRepository.AddAsync(message);

            response.Add(new FilePathDto
            {
                Path = storagePath
            });
        }

        await _unitOfWork.SaveChangesAsync();

        return new SuccessDataResult<List<FilePathDto>>(response, "Files uploaded successfully.");
    }

    public async Task<IResult> DeleteAnyMessage(DeleteMessageDto dto)
    {
        // Kullanıcının bu mesajı silme yetkisi var mı kontrol et
        var userConversation = await _unitOfWork.UserConversationRepository
            .GetAsync(uc =>
                uc.UserId == dto.MessageSenderId &&
                uc.ConversationId == dto.ConversationId);
        if (userConversation == null)
        {
            return new ErrorResult("Bu sohbette bulunmuyorsunuz.");
        }
        // Mesajı bul
        var message = await _unitOfWork.MessageRepository.GetAsync(
            predicate: i => i.Id == dto.MessageId && i.ConversationId == dto.ConversationId,
            include: x=> x.Include(x=>x.MediaContent));
        if (message == null)
        {
            return new ErrorResult("Mesaj bulunamadı.");
        }
        // Mesajı sadece gönderen silebilir
        if (message.SenderId != dto.MessageSenderId)
        {
            return new ErrorResult("Bu mesajı silme yetkiniz yok.");
        }
        try
        {
            // Eğer mesajın medya içeriği varsa, önce dosyayı sil
            if (message.MediaContent != null)
            {
                await _fileStorageService.DeleteFileAsync(message.MediaContent.StoragePath);
                await _unitOfWork.MediaContentRepository.HardDeleteAsync(message.MediaContent);
            }
            // Mesajı veritabanından sil
            await _unitOfWork.MessageRepository.HardDeleteAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult("Mesaj başarıyla silindi.");
        }
        catch (Exception ex)
        {
            // Hata loglama yapılabilir
            return new ErrorResult("Mesaj silinirken bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<IResult> DeleteConversation(DeleteGroupDto dto)
    {
        // Grubu ve ilişkili tüm verileri getir
        var conversation = await _unitOfWork.ConversationRepository.GetAsync(
            c => c.Id == dto.ConversationId,
            include: q => q
                .Include(c => c.Participants)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.MediaContent)
        );

        if (conversation == null)
            throw new NotFoundException("Conversation not found.");

        try
        {
            // Tüm medya dosyalarını sil
            foreach (var message in conversation.Messages.Where(m => m.MediaContent != null).ToList())
            {
                await _fileStorageService.DeleteFileAsync(message.MediaContent.StoragePath);
                // Mesajdan media content ilişkisini kaldır
                await _unitOfWork.MediaContentRepository.HardDeleteAsync(message.MediaContent);
                message.MediaContent = null;
            }

            // Grup mesajlarını sil
            foreach (var message in conversation.Messages.ToList())
            {
                conversation.Messages.Remove(message);
                await _unitOfWork.MessageRepository.HardDeleteAsync(message);
            }

            // Grup katılımcılarını sil
            foreach (var participant in conversation.Participants.ToList())
            {
                conversation.Participants.Remove(participant);
                await _unitOfWork.UserConversationRepository.HardDeleteAsync(participant);
            }

            // Grubu sil
            await _unitOfWork.ConversationRepository.HardDeleteAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("Group deleted successfully.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while deleting group: {ex.Message}");
        }
    }

    public async Task<IResult> DeleteUserFromGroup(DeleteUserFromGroupDto dto)
    {
        // Grubu ve katılımcıları getir
        var conversation = await _unitOfWork.ConversationRepository.GetAsync(
            c => c.Id == dto.ConversationId,
            include: q => q
                .Include(c => c.Participants)
                .Include(c => c.Messages.Where(m => m.SenderId == dto.UserId))
                    .ThenInclude(m => m.MediaContent)
        );

        if (conversation == null)
            throw new NotFoundException("Conversation not found.");

        // Kullanıcının grupta olup olmadığını kontrol et
        var userConversation = conversation.Participants
            .FirstOrDefault(p => p.UserId == dto.UserId);

        if (userConversation == null)
            return new ErrorResult("User is not in this group.");

        try
        {
            // Kullanıcının mesajlarını işle (opsiyonel - mesajları silmek istiyorsanız)
            var userMessages = conversation.Messages.Where(m => m.SenderId == dto.UserId).ToList();
            foreach (var message in userMessages)
            {
                if (message.MediaContent != null)
                {
                    await _fileStorageService.DeleteFileAsync(message.MediaContent.StoragePath);
                    await _unitOfWork.MediaContentRepository.DeleteAsync(message.MediaContent);
                    message.MediaContent = null;
                }
                conversation.Messages.Remove(message);
                await _unitOfWork.MessageRepository.HardDeleteAsync(message);
            }

            // Kullanıcıyı gruptan çıkar
            conversation.Participants.Remove(userConversation);
            await _unitOfWork.UserConversationRepository.HardDeleteAsync(userConversation);

            // Conversation'ı güncelle
            await _unitOfWork.ConversationRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("User removed from group successfully.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while removing user: {ex.Message}");
        }
    }

    public async Task<IResult> ClearMessagesConversation(ClearChatDto dto)
    {
        // Conversation'ı ve ilişkili mesajları getir
        var conversation = await _unitOfWork.ConversationRepository.GetAsync(
            c => c.Id == dto.ConversationId,
            include: q => q
                .Include(c => c.Messages)
                    .ThenInclude(m => m.MediaContent)
        );

        if (conversation == null)
            throw new NotFoundException("Conversation not found.");

        try
        {
            // Önce medya dosyalarını dosya sisteminden sil
            var messagesWithMedia = conversation.Messages
                .Where(m => m.MediaContent != null)
                .ToList();

            foreach (var message in messagesWithMedia)
            {
                var mediaContent = message.MediaContent;
                if (mediaContent != null)
                {
                    await _fileStorageService.DeleteFileAsync(mediaContent.StoragePath);
                    await _unitOfWork.MediaContentRepository.HardDeleteAsync(mediaContent);
                    message.MediaContent = null;
                }
            }

            // Tüm mesajları sil
            foreach (var message in conversation.Messages.ToList())
            {
                conversation.Messages.Remove(message);
                await _unitOfWork.MessageRepository.HardDeleteAsync(message);
            }

            // Conversation'ı güncelle
            await _unitOfWork.ConversationRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("All messages have been cleared successfully.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while clearing messages: {ex.Message}");
        }
    }
}

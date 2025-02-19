using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SikiSokoChatApp.Application.Abstractions.Repositories;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Features.Conversations.DTOs;
using SikiSokoChatApp.Domain.Entities;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Types;
using SikiSokoChatApp.Shared.Utils.Responses;
using SikiSokoChatApp.Shared.Utils.Results.Abstract;
using SikiSokoChatApp.Shared.Utils.Results.Concrete;

namespace SikiSokoChatApp.Application.Features;
public class ConversationsService : IConversationService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConversationsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
            if (existingConversation != null)
                throw new Exception("You already have conversation with user.");
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
        // 3️⃣ Eğer özel mesajsa, alıcı kontrolü
        if (!conversation.IsGroup && dto.ReceiverId == null)
            throw new Exception("ReceiverId is required for private messages.");

        //  Yeni mesajı oluştur
        var message = new Message
        {
            SenderId = dto.SenderId,
            ConversationId = dto.ConversationId,
            ReciverId = 999999, // Eğer grup mesajıysa 0 olsun
            Content = dto.Content ?? string.Empty,
            MessageContent = dto.MediaContent,
            Type = dto.Type,
            CreatedDate = DateTime.UtcNow
        };
        await _unitOfWork.MessageRepository.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult("OkiDoki.");

    }

    public async Task<IDataResult<ConversationForGetMessagesDto>> GetMessagesFromConvarsation(GetMessagesFromConversationDto dto)
    {
        // 1️⃣ Kullanıcının o konuşmaya dahil olup olmadığını kontrol et
        var conversation = await _unitOfWork.ConversationRepository.GetAsync(
            c => c.Id == dto.ConversationId,
            include: q => q.Include(c => c.Participants).ThenInclude(p => p.User));
        if (conversation == null)
            throw new NotFoundException("Conversation not found.");
        bool isParticipant = conversation.Participants.Any(p => p.UserId == dto.SenderId);
        if (!isParticipant)
            throw new UnauthorizedAccessException("User is not a participant of this conversation.");
        // 2️⃣ Konuşmaya ait mesajları getir
        var messages = await _unitOfWork.MessageRepository.GetListWithProjectionAsync(
            isAll: true,
            selector: m => new MessageDto
            {
                MessageId = m.Id,
                SenderId = m.SenderId,
                SenderName = m.Sender.Username, // Sender'ın adını al
                ReceiverId = m.ReciverId,
                Content = m.Content,
                Base64MediaContent = m.MessageContent != null ? Convert.ToBase64String(m.MessageContent) : null,
                Type = m.Type,
                CreatedDate = m.CreatedDate,
               
            },
            predicate: m => m.ConversationId == dto.ConversationId, // Belirtilen konuşmaya ait mesajları getir
            orderBy: q => q.OrderBy(m => m.CreatedDate) // Mesajları tarih sırasına göre sırala
        );

        // 🔥 Participants'ı tek seferde al ve DTO'ya ekle
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
}

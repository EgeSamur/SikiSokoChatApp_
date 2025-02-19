using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Application.Features.Conversations.DTOs;
using SikiSokoChatApp.Application.Features.Users.DTOs;

namespace SikiSokoChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _service;

        public ConversationsController(IConversationService conversationService)
        {
            _service = conversationService;
        }
        [HttpPost("create-conversation")]
        public async Task<IActionResult> CreateOneToOneConversation(CreateOneToOneConversationDto dto)
        {
            var result = await _service.CreateOneToOneConversationAsync(dto);
            return Ok(result);
        }
        [HttpPost("get-conversations")]
        public async Task<IActionResult> GetConversations(GetConversationsDto dto)
        {
            var result = await _service.GetConvarsations(dto);
            return Ok(result);
        }
        [HttpPost("add-message-conversation")]
        public async Task<IActionResult> AddMessageToConversation(SendMessageDto dto)
        {
            var result = await _service.AddMessageToConversationAsync(dto);
            return Ok(result);
        }
        [HttpPost("get-message-from-conversation")]
        public async Task<IActionResult> GetMessageFromConversation(GetMessagesFromConversationDto dto)
        {
            var result = await _service.GetMessagesFromConvarsation(dto);
            return Ok(result);
        }
        [HttpPost("add-person-to-conversation")]
        public async Task<IActionResult> AddPersonToConversation(AddPersonToConversationDto dto)
        {
            var result = await _service.AddPersonToConversationAsync(dto);
            return Ok(result);
        }
    }
}

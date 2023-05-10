using MBS.Domain.Entities;
using MBS.Host.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService messageService;

    public MessageController(IMessageService messageService)
    {
        this.messageService = messageService;
    }

    // Получить все сообщения между пользователями
    [HttpGet("messages/{user1}/{user2}")]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync(string user1, string user2)
    {
        var messages = await messageService.GetMessagesAsync(user1, user2);
        return Ok(messages);
    }

    // Получить последнее сообщение между пользователями
    [HttpGet("latest-message/{user1}/{user2}")]
    public async Task<ActionResult<Message>> GetLatestMessageAsync(string user1, string user2)
    {
        var latestMessage = await messageService.GetLatestMessageAsync(user1, user2);
        return Ok(latestMessage);
    }

    // Отправить сообщение
    [HttpPost("send-message")]
    public async Task<ActionResult<Message>> SendMessageAsync([FromBody] Message message)
    {
        var savedMessage = await messageService.SendMessageAsync(message);
        return Ok(savedMessage);
    }

    // Обновить сообщение
    [HttpPut("update-message/{messageId}")]
    public async Task<IActionResult> UpdateMessageAsync(int messageId, [FromBody] string updatedText)
    {
        await messageService.UpdateMessageAsync(messageId, updatedText);
        return NoContent();
    }
}
using MBS.Domain.Entities;
using MBS.Host.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Host.Controllers;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly IMessageService messageService;

    public MessageController(IMessageService messageService)
    {
        this.messageService = messageService;
    }

    // Получить все сообщения между пользователями
    [HttpGet("messages")]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync(string user1, string user2)
    {
        var username = this.User.Identity!.Name;
        var messages = await messageService.GetMessagesBetweenUsersAsync(user1, user2);
        return Ok(messages);
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
        await messageService.EditMessageTextAsync(messageId, updatedText);
        return NoContent();
    }
}
namespace MBS.Host.Controllers;

using MBS.Application.Dtos;
using MBS.Application.Services;
using MBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly IMessageService messageService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageController"/>.
    /// </summary>
    /// <param name="messageService">Сервис сообщений.</param>
    public MessageController(IMessageService messageService)
    {
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <summary>
    /// Получает сообщения между двумя пользователями.
    /// </summary>
    /// <param name="otherUser">Другой пользователь.</param>
    /// <returns>Сообщения.</returns>
    [HttpGet("messages")]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsersAsync([FromQuery] string otherUser)
    {
        var username = this.User.Identity!.Name;
        var messages = await this.messageService.GetMessagesBetweenUsersAsync(username, otherUser);
        return this.Ok(messages);
    }

    /// <summary>
    /// Отправляет сообщение.
    /// </summary>
    /// <param name="dto">DTO отправки сообщения.</param>
    /// <returns>Результат отправки сообщения.</returns>
    [HttpPost("message/send")]
    public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageDto dto)
    {
        var username = this.User.Identity!.Name;
        try
        {
            await this.messageService.SendMessageAsync(username, dto);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Редактирует текст сообщения.
    /// </summary>
    /// <param name="dto">DTO редактирования текста сообщения.</param>
    /// <returns>Результат редактирования текста сообщения.</returns>
    [HttpPut("message/edit-text")]
    public async Task<IActionResult> EditMessageTextAsync([FromBody] EditMessageTextDto dto)
    {
        var username = this.User.Identity!.Name;
        try
        {
            await this.messageService.EditMessageTextAsync(username, dto);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
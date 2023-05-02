/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBS.Host.Dtos;
using MBS.Domain.Entities;
using MBS.Domain.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public MessagesController(IMessageService messageService, IUserService userService, IUnitOfWork unitOfWork)
    {
        _messageService = messageService;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages()
    {
        // Реализуйте логику получения списка сообщений для текущего пользователя
        int userId = int.Parse(User.Identity.Name);
        var messages = await _messageService.GetMessagesForUserAsync(userId);

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] Message message)
    {
        // Получаем идентификатор текущего пользователя из аутентификационных данных
        int senderId = int.Parse(User.Identity.Name);

        // Отправляем сообщение от текущего пользователя другому пользователю
        // и сохраняем его в базе данных в зашифрованном виде
        var sentMessage = await _messageService.SendMessageAsync(senderId, message.ReceiverId, message.EncryptedText);

        return Ok(sentMessage);
    }
}*/
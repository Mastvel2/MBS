using MBS.Domain.Entities;
using MBS.Domain.Services;
using MBS.Host.ApplicationServices;
using Microsoft.AspNetCore.SignalR;

namespace MBS.Host.Hubs;

public class MbsHub : Hub
{
    private readonly IMessageService messageService;
    private readonly IUserService userService;


    public MbsHub(IMessageService messageService, IUserService userService)
    {
        this.messageService = messageService;
        this.userService = userService;
    }

    public async Task SendMessage(string sender, string receiver, string text)
    {
        var message = new Message
        {
            Sender = sender,
            Receiver = receiver,
            EncryptedText = text,
            Timestamp = DateTime.UtcNow
        };

        var savedMessage = await messageService.SendMessageAsync(message);
        savedMessage.EncryptedText = AesEncryption.Decrypt(savedMessage.EncryptedText);

        // Отправляем сообщение отправителю
        await Clients.User(sender).SendAsync("ReceiveMessage", savedMessage);
        // Отправляем сообщение получателю
        await Clients.User(receiver).SendAsync("ReceiveMessage", savedMessage);
    }

    public async Task<IEnumerable<Message>> LoadMessages(string user1, string user2)
    {
        var messages = await messageService.GetMessagesAsync(user1, user2);
        return messages;
    }

    public override async Task OnConnectedAsync()
    {
        string username = Context.User.Identity.Name;

        await userService.UpdateStatus(username, "online");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string username = Context.User.Identity.Name;

        await userService.UpdateStatus(username, "offline");
        await userService.UpdateLastLoginTime(username, DateTime.UtcNow);
        await base.OnDisconnectedAsync(exception);
    }
}
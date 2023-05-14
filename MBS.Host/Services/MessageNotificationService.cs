namespace MBS.Host.Services;

using MBS.Application.Dtos;
using MBS.Application.Services;
using MBS.Host.Hubs;
using Microsoft.AspNetCore.SignalR;

public class MessageNotificationService : IMessageNotificationService
{
    private readonly IHubContext<MessageHub, IMessageNotificationClient> hubContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageNotificationService"/>.
    /// </summary>
    public MessageNotificationService(IHubContext<MessageHub, IMessageNotificationClient> hubContext)
    {
        this.hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    /// <inheritdoc />
    public Task NotifyBySendMessage(string receiver, SentMessageNotificationDto dto)
    {
        return this.hubContext.Clients.Group(receiver).NotifyBySendMessage(dto);
    }

    /// <inheritdoc />
    public Task NotifyByEditMessage(string receiver, EditMessageNotificationDto dto)
    {
        return this.hubContext.Clients.Group(receiver).NotifyByEditMessage(dto);
    }
}
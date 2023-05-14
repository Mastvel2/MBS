namespace MBS.Host.Hubs;

using MBS.Application.Dtos;

/// <summary>
/// Клиент уведомлений о сообщениях.
/// </summary>
public interface IMessageNotificationClient
{
    /// <summary>
    /// Уведомить об отправке сообщения.
    /// </summary>
    /// <param name="dto">DTO уведомления об отправленном сообщении.</param>
    /// <returns>Выполняемая задача.</returns>
    Task NotifyBySendMessage(SentMessageNotificationDto dto);

    /// <summary>
    /// Уведомить о редактировании сообщения.
    /// </summary>
    /// <param name="dto">DTO уведомления о редактировании сообщения.</param>
    /// <returns>Выполняемая задача.</returns>
    Task NotifyByEditMessage(EditMessageNotificationDto dto);
}
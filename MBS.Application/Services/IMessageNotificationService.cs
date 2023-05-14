namespace MBS.Application.Services;

using MBS.Application.Dtos;

/// <summary>
/// Сервис уведомлений о сообщениях.
/// </summary>
public interface IMessageNotificationService
{
    /// <summary>
    /// Уведомляет об отправленном сообщении.
    /// </summary>
    /// <param name="receiver">Получатель сообщения.</param>
    /// <param name="dto">DTO уведомления о сообщении.</param>
    /// <returns>Выполняемая задача.</returns>
    Task NotifyBySendMessage(string receiver, SentMessageNotificationDto dto);

    /// <summary>
    /// Уведомляет о редактировании сообщения.
    /// </summary>
    /// <param name="receiver">Получатель сообщения.</param>
    /// <param name="dto">DTO уведомления о редактировании сообщения.</param>
    /// <returns>Выполняемая задача.</returns>
    Task NotifyByEditMessage(string receiver, EditMessageNotificationDto dto);
}
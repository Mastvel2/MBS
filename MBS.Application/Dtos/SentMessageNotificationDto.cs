namespace MBS.Application.Dtos;

/// <summary>
/// DTO уведомления об отправленном сообщении.
/// </summary>
public class SentMessageNotificationDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Отправитель.
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// Текст.
    /// </summary>
    public string Text { get; set; }
}
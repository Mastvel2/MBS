namespace MBS.Application.Dtos;

/// <summary>
/// DTO уведомления о редактировании сообщения.
/// </summary>
public class EditMessageNotificationDto
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
    /// Текст сообщения.
    /// </summary>
    public string Text { get; set; }
}
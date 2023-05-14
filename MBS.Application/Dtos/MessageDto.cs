namespace MBS.Application.Dtos;

/// <summary>
/// DTO сообщения.
/// </summary>
public class MessageDto
{
    /// <summary>
    /// Идентификатор сообщения.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Отправитель.
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// Получатель.
    /// </summary>
    public string Receiver { get; set; }

    /// <summary>
    /// Текст.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Время.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
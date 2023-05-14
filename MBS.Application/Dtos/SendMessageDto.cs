namespace MBS.Application.Dtos;

/// <summary>
/// DTO отправки сообщения.
/// </summary>
public class SendMessageDto
{
    /// <summary>
    /// Получатель.
    /// </summary>
    public string Receiver { get; set; }

    /// <summary>
    /// Текст.
    /// </summary>
    public string Text { get; set; }
}
namespace MBS.Application.Dtos;

/// <summary>
/// DTO редактирования текста сообщения.
/// </summary>
public class EditMessageTextDto
{
    /// <summary>
    /// Идентификатор сообщения.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Отредактированный текст.
    /// </summary>
    public string Text { get; set; }
}
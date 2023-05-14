namespace MBS.Application.Services;

using MBS.Application.Dtos;

/// <summary>
/// Сервис сообщений.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Получает сообщения между двумя пользователями.
    /// </summary>
    /// <param name="firstUser">Первый пользователь.</param>
    /// <param name="secondUser">Второй пользователь.</param>
    /// <returns>Сообщения.</returns>
    Task<IEnumerable<MessageDto>> GetMessagesBetweenUsersAsync(string firstUser, string secondUser);

    /// <summary>
    /// Отправляет сообщение.
    /// </summary>
    /// <param name="sender">Отправитель.</param>
    /// <param name="dto">DTO отправки сообщения.</param>
    /// <returns>Выполняемая задача.</returns>
    Task SendMessageAsync(string sender, SendMessageDto dto);

    /// <summary>
    /// Редактирует текст сообщения.
    /// </summary>
    /// <param name="sender">Отправитель сообщения.</param>
    /// <param name="dto">DTO редактирования текста сообщения.</param>
    /// <returns>Выполняемая задача.</returns>
    Task EditMessageTextAsync(string sender, EditMessageTextDto dto);

    Task<string> UploadFileAsync(string fileName, Stream file);
    Task<byte[]> DownloadFileAsync(string fileName);

    Task DeleteMessageAsync(Guid id);
}
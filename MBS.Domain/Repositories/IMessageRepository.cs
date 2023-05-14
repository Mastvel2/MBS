using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

/// <summary>
/// Репозиторий сообщений.
/// </summary>
public interface IMessageRepository
{
    /// <summary>
    /// Получает сообщения между двумя пользователями.
    /// </summary>
    /// <param name="firstUser">Первый пользователь.</param>
    /// <param name="secondUser">Второй пользователь.</param>
    /// <returns>Сообщения.</returns>
    IAsyncEnumerable<Message> GetMessagesBetweenUsersAsync(string firstUser, string secondUser);

    /// <summary>
    /// Получает сообщение по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Сообщение.</returns>
    Task<Message> GetByIdAsync(Guid id);

    /// <summary>
    /// Добавляет сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    void Add(Message message);

    /// <summary>
    /// Обновляет сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    void Update(Message message);

    /// <summary>
    /// Удаляет сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    void Delete(Message message);
}
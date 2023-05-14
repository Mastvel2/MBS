using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

/// <summary>
/// Репозиторий пользователей.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получает всех пользователей.
    /// </summary>
    /// <returns>Пользователи.</returns>
    IAsyncEnumerable<User> GetAllAsync();

    /// <summary>
    /// Ищет пользователей по критерию.
    /// </summary>
    /// <param name="searchTerm">Критерий поиска.</param>
    /// <returns>Пользователи.</returns>
    IAsyncEnumerable<User> SearchUsersAsync(string searchTerm);

    /// <summary>
    /// Получает пользователей по их логинам.
    /// </summary>
    /// <param name="usernames">Логины.</param>
    /// <returns>Пользователи.</returns>
    IAsyncEnumerable<User> GetByUsernamesAsync(IEnumerable<string> usernames);

    /// <summary>
    /// Получает пользователя по его логину.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <returns>Пользователь.</returns>
    Task<User> GetByUsernameAsync(string username);

    /// <summary>
    /// Получает признак наличия пользователя по его логину.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <returns>Признак наличия пользователя.</returns>
    Task<bool> HasByUsernameAsync(string username);

    /// <summary>
    /// Добавляет пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    void Add(User user);

    /// <summary>
    /// Обновляет пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    void Update(User user);
}
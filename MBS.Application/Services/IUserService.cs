namespace MBS.Application.Services;

using MBS.Application.Dtos;

/// <summary>
/// Сервис пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получает список доступных пользователей для текущего.
    /// </summary>
    /// <param name="currentUser">Текущий пользователь.</param>
    /// <returns>Доступные пользователи.</returns>
    Task<IEnumerable<UserDto>> GetAvailableUsersAsync(string currentUser);

    /// <summary>
    /// Получает пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <returns>Пользователь.</returns>
    Task<UserDto> GetUserAsync(string username);

    /// <summary>
    /// Обновляет пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <param name="dto">DTO обновления пользователя.</param>
    /// <returns>Выполняемая задача.</returns>
    Task UpdateUserAsync(string username, UserUpdateDto dto);

    /// <summary>
    /// Обновляет аватар пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <param name="fileName">Наименование файла.</param>
    /// <param name="avatarFile">Файл аватара.</param>
    /// <returns>Выполняемая задача.</returns>
    Task UpdateUserAvatarAsync(string username, string fileName, Stream avatarFile);

    /// <summary>
    /// Обновляет время последней активности пользователей.
    /// </summary>
    /// <returns>Выполняемая задача.</returns>
    Task UpdateLastActiveTimesAsync();
}
using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

/// <summary>
/// Репозиторий идентификационных данных пользователя.
/// </summary>
public interface IUserIdentityRepository
{
    /// <summary>
    /// Получает идентификационные данные по логину.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <returns>Идентификационные данные пользователя.</returns>
    Task<UserIdentity> GetByUsernameAsync(string username);

    /// <summary>
    /// Проверяет наличие идентификационных данных по логину.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <returns>Признак наличия идентификационных данных.</returns>
    Task<bool> HasByUsernameAsync(string username);

    /// <summary>
    /// Добавляет идентификационные данные.
    /// </summary>
    /// <param name="userIdentity">Идентификационные данные пользователя.</param>
    void Add(UserIdentity userIdentity);
}
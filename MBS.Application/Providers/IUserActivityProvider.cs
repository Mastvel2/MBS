namespace MBS.Application.Providers;

/// <summary>
/// Провайдер активности пользователей.
/// </summary>
public interface IUserActivityProvider
{
    /// <summary>
    /// Получает зарегистрированные активности пользователей.
    /// </summary>
    /// <returns>Логин и время последней активности.</returns>
    IDictionary<string, DateTime> GetActivities();

    /// <summary>
    /// Получает активность пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <returns>Дата активности.</returns>
    DateTime? GetActivity(string username);

    /// <summary>
    /// Обновляет активность пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    void UpdateActivity(string username);

    /// <summary>
    /// Удаляет активность пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    void DeleteActivity(string username);
}
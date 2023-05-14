namespace MBS.Application.Services;

/// <summary>
/// Фабрика токенов.
/// </summary>
public interface ITokenFactory
{
    /// <summary>
    /// Создает токен.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <param name="isAdmin">Признак администратора.</param>
    /// <returns>Токен.</returns>
    string Create(string username, bool isAdmin);
}
namespace MBS.Application.Services;

using MBS.Application.Dtos;

/// <summary>
/// Сервис идентификационных данных пользователя.
/// </summary>
public interface IUserIdentityService
{
    /// <summary>
    /// Регистрирует пользователя.
    /// </summary>
    /// <param name="dto">DTO регистрации пользователя.</param>
    /// <returns>Выполняемая задача.</returns>
    Task RegisterAsync(UserRegistrationDto dto);

    /// <summary>
    /// Авторизирует пользователя.
    /// </summary>
    /// <param name="dto">DTO авторизации пользователя.</param>
    /// <returns>Токен.</returns>
    Task<TokenDto> AuthorizeAsync(UserAuthorizationDto dto);
}
namespace MBS.Application.Dtos;

/// <summary>
/// DTO авторизации пользователя.
/// </summary>
public class UserAuthorizationDto
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }
}
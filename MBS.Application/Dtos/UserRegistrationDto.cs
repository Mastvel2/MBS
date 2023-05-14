namespace MBS.Application.Dtos;

/// <summary>
/// DTO регистрации пользователя.
/// </summary>
public class UserRegistrationDto
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
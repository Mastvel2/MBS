namespace MBS.Application.Dtos;

/// <summary>
/// DTO пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Обо мне.
    /// </summary>
    public string AboutMe { get; set; }

    /// <summary>
    /// Статус пользователя.
    /// </summary>
    public UserStatus Status { get; set; }

    /// <summary>
    /// Последняя дата активности.
    /// </summary>
    public DateTime LastActivityTime { get; set; }
}
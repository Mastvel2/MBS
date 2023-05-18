namespace MBS.Application.Dtos;

/// <summary>
/// DTO обновления пользователя.
/// </summary>
public class UserUpdateDto
{
    /// <summary>
    /// Обо мне.
    /// </summary>
    public string AboutMe { get; set; }

    /// <summary>
    /// Отображаемое имя.
    /// </summary>
    public string DisplayName { get; set; }
}
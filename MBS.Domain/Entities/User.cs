namespace MBS.Domain.Entities;

/// <summary>
/// Пользователь.
/// </summary>
public class User
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="User"/>.
    /// </summary>
    /// <param name="username">Логин.</param>
    public User(string username)
    {
        this.Username = username;
    }

    /// <summary>
    /// Логин.
    /// </summary>
    public string Username { get; protected set; }

    /// <summary>
    /// Обо мне.
    /// </summary>
    public string AboutMe { get; set; }

    /// <summary>
    /// Последнее время активности.
    /// </summary>
    public DateTime? LastActivityTime { get; set; }
}
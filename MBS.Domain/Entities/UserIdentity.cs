namespace MBS.Domain.Entities;

/// <summary>
/// Идентификационные данные пользователя.
/// </summary>
public class UserIdentity
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserIdentity"/>.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <param name="clearPassword">Пароль в чистом виде.</param>
    public UserIdentity(string username, string clearPassword)
    {
        this.Username = username;
        this.Password = new Password(clearPassword);
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserIdentity"/>.
    /// </summary>
    [Obsolete("ORM Only")]
    protected UserIdentity()
    {
    }

    /// <summary>
    /// Логин.
    /// </summary>
    public string Username { get; protected set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public Password Password { get; }

    /// <summary>
    /// Признак администратора.
    /// </summary>
    public bool IsAdmin { get; } = false;
}
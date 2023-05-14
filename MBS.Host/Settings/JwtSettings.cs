namespace MBS.Host.Settings;

/// <summary>
/// Настройки JWT.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Наименование секции конфигурации.
    /// </summary>
    public const string SectionName = "JwtSettings";

    /// <summary>
    /// Ключ секрета.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// Длительность в минутах.
    /// </summary>
    public int ExpirationMinutes { get; set; }
}
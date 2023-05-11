namespace MBS.Domain.Entities;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string SecretKey { get; set; }
    public int ExpirationMinutes { get; set; }
}
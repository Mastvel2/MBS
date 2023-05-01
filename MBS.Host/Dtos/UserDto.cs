namespace MBS.Host.Dtos;

public class UserDto
{
    public string Password { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}
namespace MBS.Host.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}
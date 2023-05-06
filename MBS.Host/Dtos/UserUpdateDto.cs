namespace MBS.Host.Dtos;

public class UserUpdateDto
{
    public string Username { get; set; }

    public string AboutMe { get; set; }

    public string ProfilePictureUrl { get; set; }

    public DateTime LastLoginTime { get; set; }

    public string Status { get; set; }
}
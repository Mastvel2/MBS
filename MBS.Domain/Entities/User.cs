namespace MBS.Domain.Entities;

public class User
{

    public User(string username)
    {
        this.Username = username;
    }
    
    public string Username { get; set; }

    public string AboutMe { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public string ProfilePictureUrl { get; set; }

    public DateTime LastLogin { get; set; }
}
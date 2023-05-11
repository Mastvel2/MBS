namespace MBS.Domain.Entities;

public class User
{

    public User(string username)
    {
        this.Username = username;
    }

    public string Username { get; protected set; }

    public string AboutMe { get; set; }

    public DateTime LastActiveTime { get; set; }

}
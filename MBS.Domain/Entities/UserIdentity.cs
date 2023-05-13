namespace MBS.Domain.Entities;

public class UserIdentity
{
    public UserIdentity(string username, string clearPassword)
    {
        this.Username = username;
        this.Password = new Password(clearPassword);
    }

    protected UserIdentity()
    {
    }

    public string Username { get; protected set; }

    public Password Password { get; }

    public bool IsAdmin { get; } = false;
}
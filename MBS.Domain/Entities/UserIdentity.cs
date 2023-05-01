namespace MBS.Domain.Entities;

public class UserIdentity
{
    private string hash;
    private string salt;
    
    public UserIdentity(string username, string clearPassword)
    {
        Username = username;
        this.Password = new Password(clearPassword);
    }
    
    protected UserIdentity()
    {
        this.Password = new Password(this.salt, this.hash);
    }
    
    public string Username { get; protected set; }
    
    public Password Password { get; }
}
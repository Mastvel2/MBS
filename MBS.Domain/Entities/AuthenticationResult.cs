namespace MBS.Domain;

public class AuthenticationResult
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public string Token { get; set; }
}
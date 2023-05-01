namespace MBS.Host.InfrastructureServices;

public interface ITokenFactory
{
    string Create(string username);
}
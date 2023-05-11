using MBS.Domain.Entities;

namespace MBS.Host.Providers;

public interface IUserStatusProvider
{
    UserStatus GetStatus(string username);
    void UpdateStatus(string username);
}
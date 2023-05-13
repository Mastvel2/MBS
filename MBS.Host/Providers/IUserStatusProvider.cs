using MBS.Domain.Entities;

namespace MBS.Host.Providers;

public interface IUserStatusProvider
{
    IDictionary<string, DateTime> GetActivities();
    UserStatus GetStatus(string username);
    void UpdateStatus(string username);
}
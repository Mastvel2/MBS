using System.Collections.Concurrent;
using MBS.Domain.Entities;

namespace MBS.Host.Providers;

public class UserStatusProvider:IUserStatusProvider
{
    private static readonly TimeSpan MaxInactiveTime = new TimeSpan(0, 5, 0);
    private readonly ConcurrentDictionary<string, DateTime> activeTimes = new();

    public IDictionary<string, DateTime> GetActivities()
    {
        var result = activeTimes.ToDictionary(k => k.Key, v => v.Value);
        foreach (var (username, _) in result)
        {
            if (this.GetStatus(username) == UserStatus.Offline)
            {
                activeTimes.Remove(username, out _);
            }
        }
        return result;
    }

    public UserStatus GetStatus(string username)
    {
        if (!activeTimes.TryGetValue(username, out var activeTime))
        {
            return UserStatus.Offline;
        }

        var currentTime = DateTime.Now;
        return currentTime - activeTime < MaxInactiveTime
            ? UserStatus.Online : UserStatus.Offline;
    }

    public void UpdateStatus(string username)
    {
        this.activeTimes.AddOrUpdate(username, DateTime.Now, (_, _) => DateTime.Now);
    }
}
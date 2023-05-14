namespace MBS.Application.Providers;

using System.Collections.Concurrent;

/// <inheritdoc />
public class UserActivityProvider : IUserActivityProvider
{
    private readonly ConcurrentDictionary<string, DateTime> activeTimes = new();

    /// <inheritdoc />
    public IDictionary<string, DateTime> GetActivities()
    {
        return this.activeTimes.ToDictionary(k => k.Key, v => v.Value);
    }

    /// <inheritdoc />
    public DateTime? GetActivity(string username)
    {
        if (this.activeTimes.TryGetValue(username, out var activeTime))
        {
            return activeTime;
        }

        return null;
    }

    /// <inheritdoc />
    public void UpdateActivity(string username)
    {
        this.activeTimes.AddOrUpdate(username, DateTime.Now, (_, _) => DateTime.Now);
    }

    /// <inheritdoc />
    public void DeleteActivity(string username)
    {
        this.activeTimes.TryRemove(username, out _);
    }
}
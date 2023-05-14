namespace MBS.Host.Services;

using MBS.Application.Services;

/// <summary>
/// Фоновый сервис обновления активности пользователей.
/// </summary>
public class UserActivityBackgroundService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserActivityBackgroundService"/>.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов.</param>
    public UserActivityBackgroundService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using (var scope = this.serviceProvider.CreateAsyncScope())
            {
                var userService = scope.ServiceProvider.GetService<IUserService>();
                await userService.UpdateLastActiveTimesAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

}
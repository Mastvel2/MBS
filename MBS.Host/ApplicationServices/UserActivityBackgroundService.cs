namespace MBS.Host.ApplicationServices;

public class UserActivityBackgroundService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public UserActivityBackgroundService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    //Сохранить последнее время активности пользователей
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
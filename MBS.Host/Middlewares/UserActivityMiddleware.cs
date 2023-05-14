namespace MBS.Host.Middlewares;

using MBS.Application.Providers;

/// <summary>
/// Промежуточный слой работы с активностью пользователя.
/// </summary>
public class UserActivityMiddleware
{
    private readonly RequestDelegate next;
    private readonly IUserActivityProvider userActivityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserActivityMiddleware"/>.
    /// </summary>
    /// <param name="next">Делегат на следующий запрос.</param>
    /// <param name="userActivityProvider">Провайдер активности пользователей.</param>
    public UserActivityMiddleware(RequestDelegate next, IUserActivityProvider userActivityProvider)
    {
        this.next = next;
        this.userActivityProvider = userActivityProvider ?? throw new ArgumentNullException(nameof(userActivityProvider));
    }

    /// <summary>
    /// Выполняет промежуточный слой.
    /// </summary>
    /// <param name="context">HTTP-контекст.</param>
    public async Task Invoke(HttpContext context)
    {
        var username = context.User.Identity?.Name;
        if (username != null)
        {
            this.userActivityProvider.UpdateActivity(username);
        }

        await this.next(context);
    }
}
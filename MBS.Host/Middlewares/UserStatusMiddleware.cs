using MBS.Host.Providers;

namespace MBS.Host.Middlewares;

public class UserStatusMiddleware
{
    private readonly RequestDelegate next;
    private readonly IUserStatusProvider userStatusProvider;
    public UserStatusMiddleware(RequestDelegate next, IUserStatusProvider userStatusProvider)
    {
        this.next = next;
        this.userStatusProvider = userStatusProvider ?? throw new ArgumentNullException(nameof(userStatusProvider));
    }

    public async Task Invoke(HttpContext context)
    {
        var username = context.User.Identity?.Name;
        if (username != null)
        {
            this.userStatusProvider.UpdateStatus(username);
        }

        await this.next(context);
    }
}
namespace MBS.Host.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Концентатор работы с сообщениями.
/// </summary>
[Authorize]
public class MessageHub : Hub<IMessageNotificationClient>
{
    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        var username = this.Context.User!.Identity!.Name;
        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, username!);
        await base.OnConnectedAsync();
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var username = this.Context.User!.Identity!.Name;
        await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, username!);
        await base.OnDisconnectedAsync(exception);
    }
}
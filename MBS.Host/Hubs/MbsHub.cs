using MBS.Host.ApplicationServices;
using Microsoft.AspNetCore.SignalR;

namespace MBS.Host.Hubs;

public class MbsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var username = Context.User!.Identity!.Name;

        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, username!);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var username = Context.User!.Identity!.Name;

        await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, username!);
        await base.OnDisconnectedAsync(exception);
    }
}
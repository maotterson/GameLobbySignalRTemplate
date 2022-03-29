using Microsoft.AspNetCore.SignalR;
namespace GameLobbySignalRTemplate.SignalR.Hubs
{
    public class LobbyHub : Hub
    {
        public LobbyHub()
        {
            
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.ConnectionId;
            await Clients.All.SendAsync("UserConnected", userId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.ConnectionId;
            await Clients.All.SendAsync("UserDisconnected", userId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}

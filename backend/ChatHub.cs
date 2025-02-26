using Microsoft.AspNetCore.SignalR;

namespace SignalR_prototype;
public sealed class ChatHub : Hub<IChatClient>
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.ReceiveMessage(user, message);
    }
}


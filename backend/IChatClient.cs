namespace SignalR_prototype;
public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}

namespace SignalR_prototype;
public interface IChatClient
{
    Task ReceiveMessage(string message);
    Task SendMessage(string message);
}

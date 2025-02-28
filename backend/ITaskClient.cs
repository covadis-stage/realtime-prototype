using SignalRPrototype.Backend;

namespace SignalR_prototype;
public interface ITaskClient
{
    Task ReceiveTask(ProjectTask task);
}

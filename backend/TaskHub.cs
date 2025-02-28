using Microsoft.AspNetCore.SignalR;
using SignalRPrototype.Backend;

namespace SignalR_prototype;
public sealed class TaskHub : Hub<ITaskClient>
{
    private readonly TaskService _taskService;

    public TaskHub(TaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task UpdateTask(ProjectTask task)
    {
        await _taskService.UpdateTaskAsync(task);
        await Clients.All.ReceiveTask(task);
    }
}


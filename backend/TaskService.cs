using Microsoft.EntityFrameworkCore;

namespace SignalRPrototype.Backend;

public class TaskService
{
    public readonly AppDbContext _dbContext;

    public TaskService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksAsync()
    {
        return await _dbContext.ProjectTasks.ToListAsync();
    }

    public async Task<ProjectTask?> GetTaskAsync(int id)
    {
        return await _dbContext.ProjectTasks.FindAsync(id);
    }

    public async Task<ProjectTask> UpdateTaskAsync(ProjectTask task)
    {
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
    {
        _dbContext.ProjectTasks.Add(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }
}
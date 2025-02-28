using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR_prototype;
using SignalRPrototype.Backend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalHost",
        policy => policy
        .WithOrigins("http://localhost:5173", "https://localhost:7229")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalHost");

app.MapPost("broadcast", async (string message, IHubContext<TaskHub, ITaskClient> context) => {
    // await context.Clients.All.ReceiveMessage("Server", message);
    return Results.Ok();
});

app.MapGet("tasks", async (TaskService taskService) => {
    var tasks = await taskService.GetTasksAsync();
    return Results.Ok(tasks);
});

app.MapPost("tasks", async (ProjectTask task, TaskService taskService, IHubContext<TaskHub, ITaskClient> context) => {
    task.SelectedBy = null;
    var createdTask = await taskService.CreateTaskAsync(task);
    await context.Clients.All.ReceiveTask(createdTask);
    return Results.Ok(createdTask);
});

app.MapPut("tasks/{id}", async (int id, ProjectTask task, TaskService taskService, IHubContext<TaskHub, ITaskClient> context) => {
    var existingTask = await taskService.GetTaskAsync(id);
    if (existingTask == null)
    {
        return Results.NotFound();
    }
    existingTask.Name = task.Name;
    existingTask.Description = task.Description;
    var updatedTask = await taskService.UpdateTaskAsync(existingTask);
    await context.Clients.All.ReceiveTask(updatedTask);
    return Results.Ok(updatedTask);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<TaskHub>("taskhub");

app.Run();

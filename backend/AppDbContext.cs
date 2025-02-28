using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace SignalRPrototype.Backend;

public class AppDbContext : DbContext
{
    public DbSet<ProjectTask> ProjectTasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
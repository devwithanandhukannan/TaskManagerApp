using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map your pure domain objects to real MongoDB Atlas collections
        modelBuilder.Entity<User>().ToCollection("users");
        modelBuilder.Entity<TaskItem>().ToCollection("tasks");

        // Unique cloud index for security
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}

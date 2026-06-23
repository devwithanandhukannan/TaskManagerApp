using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure;

public static class DependencyInjection{
    public static IServiceCollection AddInfrastructureServices( this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoAtlas");

        if (string.IsNullOrEmpty(connectionString)){
            throw new InvalidOperationException("Connection string 'MongoAtlas' not found in appsettings.json.");
        }
        services.AddDbContext<AppDbContext>(options =>options.UseMongoDB(connectionString, "TaskManagerDB"));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        return services;

    }
}
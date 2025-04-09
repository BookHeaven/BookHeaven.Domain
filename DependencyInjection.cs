using BookHeaven.Domain.Abstractions.Behaviors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookHeaven.Domain;

public static class DependencyInjection
{
    // Add migration
    // dotnet ef migrations add [MigrationName] --project BookHeaven.Domain --startup-project BookHeaven.Server

    /// <summary>
    /// Registers the database context and any other services needed for the domain layer.<br/>
    /// Applies any pending migrations on startup.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbFolder">The location of the sqlite database</param>
    /// <returns></returns>
    public static IServiceCollection AddDomain(this IServiceCollection services, string dbFolder)
    {
        services.AddDbContextFactory<DatabaseContext>(options =>
        {
            options.UseSqlite($"Data Source={Path.Combine(dbFolder, "BookHeaven.db")}");
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });
        
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });
        
        return services;
    } 
}
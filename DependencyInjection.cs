using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookHeaven.Domain.Services;

namespace BookHeaven.Domain;

public static class DependencyInjection
{
    // Add migration
    // dotnet ef migrations add [MigrationName] --project BookHeaven.Domain --startup-project BookHeaven.Server
    
    public enum DatabaseInjectionType
    {
        Service,
        Factory
    }

    /// <summary>
    /// Registers the database context and any other services needed for the domain layer.<br/>
    /// Applies any pending migrations on startup.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbFolder">The location of the sqlite database</param>
    /// <returns></returns>
    public static IServiceCollection AddDomain(this IServiceCollection services, string dbFolder, DatabaseInjectionType databaseInjectionType)
    {
        switch (databaseInjectionType)
        {
            case DatabaseInjectionType.Service:
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlite($"Data Source={Path.Combine(dbFolder, "BookHeaven.db")}");
                    #if DEBUG
                        options.EnableSensitiveDataLogging();
                    #endif
                });
                services.AddTransient<IDatabaseService, DatabaseService<DatabaseContext>>();
                break;
            case DatabaseInjectionType.Factory:
                services.AddDbContextFactory<DatabaseContext>(options =>
                {
                    options.UseSqlite($"Data Source={Path.Combine(dbFolder, "BookHeaven.db")}");
#if DEBUG
                    options.EnableSensitiveDataLogging();
#endif
                });
                break;
        }
        
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        return services;
    } 
}
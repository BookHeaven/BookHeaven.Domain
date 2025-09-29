using BookHeaven.Domain.Abstractions.Behaviors;
using BookHeaven.Domain.Services;
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
    /// <param name="booksPath">The location to store the books</param>
    /// <param name="coversPath">The location to store the covers</param>
    /// <param name="fontsPath">The location to store the fonts</param>
    /// <param name="dbFolder">The location of the sqlite database</param>
    /// <returns></returns>
    public static IServiceCollection AddDomain(this IServiceCollection services, string booksPath, string coversPath, string fontsPath, string dbFolder)
    {
        Globals.BooksPath = booksPath;
        Globals.CoversPath = coversPath;
        Globals.FontsPath = fontsPath;
        
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

        services.AddSingleton<GlobalEventsService>();
        services.AddScoped<BookManager>();
        
        return services;
    } 
}
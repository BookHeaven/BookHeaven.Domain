using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.BookSeries;

public static class AddSeries
{
    public sealed record Command(Series Series) : ICommand;

    internal class CommandHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            await context.Series.AddAsync(request.Series, cancellationToken);
            
            try 
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }
            return Result.Success();
        }
    }
}
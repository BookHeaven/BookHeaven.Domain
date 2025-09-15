using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.BookSeries;

public static class UpdateSeries {
    public sealed record Command(Series Series) : ICommand<Series>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Series>
    {
        public async Task<Result<Series>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            context.Series.Update(request.Series);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return new Error("An error occurred while updating the series");
            }

            return request.Series;
        }
    }
}
using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.BookSeries;

public static class GetAllSeries{
    public sealed record Query(bool IncludeBooks = false) : ICustomQuery<List<Series>>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICustomQueryHandler<Query, List<Series>>
    {
        public async Task<Result<List<Series>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
            return request.IncludeBooks
                ? await context.Series.Include(x => x.Books).ThenInclude(b => b.Author).ToListAsync(cancellationToken)
                : await context.Series.ToListAsync(cancellationToken);
        }
    }
}
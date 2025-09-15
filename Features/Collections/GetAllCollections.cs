using BookHeaven.Domain.Entities.Base;

namespace BookHeaven.Domain.Features.Collections;

public static class GetAllCollections
{
    public sealed record Query : IQuery<List<Collection>>;
    
    internal sealed class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, List<Collection>>
    {
        public async Task<Result<List<Collection>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Collections.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
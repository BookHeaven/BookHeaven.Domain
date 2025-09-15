using BookHeaven.Domain.Entities.Base;

namespace BookHeaven.Domain.Features.Collections;

public static class GetPinnedCollectionsByProfile
{
    public sealed record Query(Guid ProfileId) : IQuery<List<Collection>>;
    
    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, List<Collection>>
    {
        public async Task<Result<List<Collection>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Collections.Where(c => c.ProfileId == null || c.ProfileId == request.ProfileId && c.Pinned == true).OrderBy(c => c.SortOrder).ToListAsync(cancellationToken);
        }
    }
}
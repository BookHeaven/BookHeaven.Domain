using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.Tags;

public static class GetAllTags
{
    public sealed record Query : ICustomQuery<List<Tag>>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICustomQueryHandler<Query, List<Tag>>
    {
        public async Task<Result<List<Tag>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Tags.ToListAsync(cancellationToken);
        }
    }
}
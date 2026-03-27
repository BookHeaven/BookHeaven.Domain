using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.BooksProgress;

public static class GetAllBooksProgressByProfile
{
    public sealed record Query(Guid ProfileId) : IQuery<List<BookProgress>>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, List<BookProgress>>
    {
        public async Task<Result<List<BookProgress>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var progress = await context.BooksProgress.Where(p => p.ProfileId == request.ProfileId)
                .ToListAsync(cancellationToken);

            return progress.Count > 0 ? progress : new Error("Progress not found");
        }
    }
}
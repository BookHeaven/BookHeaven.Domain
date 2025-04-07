using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.BooksProgress;

public static class GetBookProgress
{
    public sealed record Query(Guid BookProgressId) : IQuery<BookProgress>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, BookProgress>
    {
        public async Task<Result<BookProgress>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var progress = await context.BooksProgress.FirstOrDefaultAsync(bp => bp.BookProgressId == request.BookProgressId, cancellationToken);

            return progress != null ? progress : new Error("Error", "Progress not found");
        }
    }
}

public static class GetBookProgressByProfile
{
    public sealed record Query(Guid BookId, Guid ProfileId) : IQuery<BookProgress>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, BookProgress>
    {
        public async Task<Result<BookProgress>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var progress = await context.BooksProgress.FirstOrDefaultAsync(x => x.BookId == request.BookId && x.ProfileId == request.ProfileId, cancellationToken);

            return progress != null ? progress : new Error("Error", "Progress not found");
        }
    }
}

using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.BooksProgress;

public static class CreateBookProgress
{
    public sealed record Command(Guid BookId, Guid ProfileId) : ICommand<Guid>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var progress = new BookProgress
            {
                BookId = request.BookId,
                ProfileId = request.ProfileId
            };

            try
            {
                await context.BooksProgress.AddAsync(progress, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }
            return progress.BookProgressId;
        }
    }
}


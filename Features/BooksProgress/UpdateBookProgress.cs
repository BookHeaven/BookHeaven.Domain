using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.BooksProgress;

public static class UpdateBookProgress
{
    public sealed record Command(BookProgress BookProgress) : ICommand;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async ValueTask<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);


            if (request.BookProgress.Progress > 100)
            {
                request.BookProgress.Progress = 100;
            }
            context.BooksProgress.Update(request.BookProgress);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return Result.Failure(new Error("An error occurred while updating the book progress"));
            }

            return Result.Success();
        }
    }
}
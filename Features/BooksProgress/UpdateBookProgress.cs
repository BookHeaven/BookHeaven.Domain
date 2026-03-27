using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.BooksProgress;

public static class UpdateBookProgress
{
    public sealed record Command(BookProgress BookProgress) : ICustomCommand;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICustomCommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
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
using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Books;

public static class DeleteBook
{
    public sealed record Command(Guid BookId) : ICommand;

    internal class CommandHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var book = await context.Books
                .FirstOrDefaultAsync(b => b.BookId == request.BookId, cancellationToken);

            if (book == null)
            {
                return new Error("NOT_FOUND", "Book not found");
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
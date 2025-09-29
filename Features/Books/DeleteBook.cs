using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Events;
using BookHeaven.Domain.Extensions;
using BookHeaven.Domain.Services;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHeaven.Domain.Features.Books;

public static class DeleteBook
{
    public sealed record Command(Guid BookId) : ICommand;

    internal class CommandHandler(
        ILogger<CommandHandler> logger,
        IDbContextFactory<DatabaseContext> dbContextFactory,
        GlobalEventsService globalEventsService) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var book = await context.Books
                .FirstOrDefaultAsync(b => b.BookId == request.BookId, cancellationToken);

            if (book == null)
            {
                return new Error("Book not found");
            }


            try
            {
                if(File.Exists(book.EbookPath())) File.Delete(book.EbookPath());
                if(File.Exists(book.CoverPath())) File.Delete(book.CoverPath());
                
                context.Books.Remove(book);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete book with ID {BookId}", request.BookId);
                return new Error("Failed to delete book");
            }
           

            await globalEventsService.Publish(new BookDeleted(request.BookId));

            return Result.Success();
        }
    }
}
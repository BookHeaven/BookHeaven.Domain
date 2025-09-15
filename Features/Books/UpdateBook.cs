using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Extensions;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Books;

public static class UpdateBook
{
    public sealed record Command(Book Book, string? CoverSourcePath, string? EpubSourcePath) : ICommand;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
            var book = await context.Books
                .FirstOrDefaultAsync(b => b.BookId == request.Book.BookId, cancellationToken);
        
            if (book == null)
            {
                return new Error("Book not found");
            }
        
            book.UpdateFrom(request.Book);
        
            if(request.Book.Author is not null && !context.Authors.Any(a => a.AuthorId == request.Book.Author.AuthorId))
            {
                book.Author = request.Book.Author;
            }
            if(request.Book.Series is not null && !context.Series.Any(s => s.SeriesId == request.Book.Series.SeriesId))
            {
                book.Series = request.Book.Series;
            }

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                await Utilities.StoreFile(request.CoverSourcePath, book.CoverPath(), cancellationToken);
                await Utilities.StoreFile(request.EpubSourcePath, book.EpubPath(), cancellationToken);
                
            }
            catch (DbUpdateException)
            {
                return new Error("An error occurred while updating the book");
            }

            return Result.Success();
        }
    }
}
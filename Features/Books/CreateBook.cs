using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Books;

[Obsolete("Use AddBook command instead")]
public static class CreateBook
{
    public sealed record Command(
        Guid AuthorId,
        Guid? SeriesId,
        decimal? SeriesIndex,
        string Title,
        string? Description,
        DateTime? PublishedDate,
        string? Publisher,
        string? Language,
        string? Isbn10,
        string? Isbn13,
        string? Asin,
        string? Uuid
    ) : ICommand<Guid>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            Book book = new()
            {
                Title = request.Title,
                Description = request.Description,
                PublishedDate = request.PublishedDate,
                Publisher = request.Publisher,
                Language = request.Language,
                AuthorId = request.AuthorId,
                SeriesId = request.SeriesId,
                SeriesIndex = request.SeriesIndex
            };
        
            await context.Books.AddAsync(book, cancellationToken);
        
            try 
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }

            return book.BookId;
        }
    }
}
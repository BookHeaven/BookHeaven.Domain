using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Books;

public static class GetBook
{
    public sealed record Query(Guid? BookId, string? Title = null): IQuery<Book>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, Book>
    {
        public async Task<Result<Book>> Handle(Query request, CancellationToken cancellationToken)
        {
            if(request.BookId == null && request.Title == null)
            {
                return new Error("Error", "You must provide either a BookId or a Title");
            }
        
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
            var book = await context.Books
                .Include(b => b.Author)
                .Include(b => b.Series)
                .Include(b => b.Tags)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.BookId == request.BookId || x.Title == request.Title, cancellationToken);
        
            return book != null ? book : new Error("Error", "Book not found");
        }
    }
}

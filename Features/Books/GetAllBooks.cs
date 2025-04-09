using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Books;

public static class GetAllBooks
{
    public sealed record Query(Guid ProfileId, string? Filter = null) : IQuery<List<Book>>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, List<Book>>
    {
        public async Task<Result<List<Book>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var books = context.Books
                .Include(b => b.Author)
                .Include(b => b.Series)
                .Include(b => b.Tags).AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                books = books.Where(b =>
                    b.Title!.ToUpper().Contains(request.Filter) ||
                    b.Author!.Name!.ToUpper().Contains(request.Filter) ||
                    b.Series!.Name!.ToUpper().Contains(request.Filter) ||
                    b.Tags.Any(t => t.Name.ToUpper().Contains(request.Filter)));
            }
            else
            {
                books = books.Include(b => b.Progresses.Where(bp => bp.ProfileId == request.ProfileId));
            }
            
            var results = await books.AsSplitQuery().ToListAsync(cancellationToken);

            return results.Count > 0 ? results : new Error("Error", "No books found");
        }
    }
}


/*public sealed record GetAllBooksContainingQuery(string Filter) : IQuery<List<Book>>;

internal class GetAllBooksContainingQueryHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<GetAllBooksContainingQuery, List<Book>>
{
    public async Task<Result<List<Book>>> Handle(GetAllBooksContainingQuery request, CancellationToken cancellationToken)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var books = await context.Books
            .Include(b => b.Author)
            .Include(b => b.Series)
            .Include(b => b.Tags)
            .Where(b => 
                b.Title!.ToUpper().Contains(request.Filter) ||
                b.Author!.Name!.ToUpper().Contains(request.Filter) ||
                b.Series!.Name!.ToUpper().Contains(request.Filter) ||
                b.Tags.Any(t => t.Name.ToUpper().Contains(request.Filter)))
            .ToListAsync(cancellationToken);
        return books.Count != 0 ? books : new Error("Error", $"No books found with filter {request.Filter}");
    }
}*/
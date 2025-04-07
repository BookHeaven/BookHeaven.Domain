using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Books;

public static class AddBook
{
    public sealed record Command(Book Book) : ICommand;

    internal class CommandHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            await context.Books.AddAsync(request.Book, cancellationToken);
            
            if(request.Book.Author is not null && context.Authors.Any(a => a.AuthorId == request.Book.Author.AuthorId))
            {
                context.Entry(request.Book.Author).State = EntityState.Detached;
            }
            if(request.Book.Series is not null && context.Series.Any(s => s.SeriesId == request.Book.Series.SeriesId))
            {
                context.Entry(request.Book.Series).State = EntityState.Detached;
            }
            
            try 
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error("Error", e.Message);
            }
            return Result.Success();
        }
    }
}
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
            request.Book.Author = null;
            request.Book.Series = null;
            await context.Books.AddAsync(request.Book, cancellationToken);
            
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
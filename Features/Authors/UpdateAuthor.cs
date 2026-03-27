using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Authors;

public static class UpdateAuthor
{
    public sealed record Command(Author Author) : ICommand<Author>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Author>
    {
        public async Task<Result<Author>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            var existingAuthor = await context.Authors.FirstOrDefaultAsync(a => a.AuthorId == request.Author.AuthorId, cancellationToken);
            if (existingAuthor == null)
            {
                return new Error("Author not found");
            }
            
            existingAuthor.Name = request.Author.Name;
            existingAuthor.Biography = request.Author.Biography;
        
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return new Error("An error occurred while updating the author");
            }
        
            return request.Author;
        }
    }
}
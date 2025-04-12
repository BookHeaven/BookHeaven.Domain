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
        
            context.Authors.Update(request.Author);
        
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
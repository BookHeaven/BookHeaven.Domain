using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities.Base;
using BookHeaven.Domain.Events;
using BookHeaven.Domain.Services;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Collections;

public static class AddCollection
{
    public sealed record Command(Collection Collection) : ICommand<Guid>;
    
    internal class Handler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        GlobalEventsService eventsService) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            var sortOrder = await dbContext.Collections.MaxAsync(c => (int?)c.SortOrder, cancellationToken) ?? 0;
            request.Collection.SortOrder = sortOrder + 1;
            
            try
            {
                await dbContext.Collections.AddAsync(request.Collection, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                return new Error("Error adding collection");
            }
            await eventsService.Publish(new CollectionCreated(request.Collection.CollectionId));
            return request.Collection.CollectionId;
        }
    }
}
﻿using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.BookSeries;

public static class CreateSeries
{
    public sealed record Command(string Name) : ICommand<Series>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Series>
    {
        public async Task<Result<Series>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            Series series = new()
            {
                Name = request.Name
            };
        
            await context.Series.AddAsync(series, cancellationToken);
        
            try 
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }

            return series;
        }
    }
}


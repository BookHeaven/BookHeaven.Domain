using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHeaven.Domain.Features.Fonts;

public static class GetAllFonts
{
    public sealed record Query(string? FamilyName = null) : IQuery<List<Font>>;

    internal class QueryHandler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<QueryHandler> logger) : IQueryHandler<Query, List<Font>>
    {
        public async Task<Result<List<Font>>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var fonts = await context.Fonts
                    .Where(f => request.FamilyName == null || f.Family == request.FamilyName)
                    .ToListAsync(cancellationToken);
                return fonts;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting fonts");
                return new Error("Error getting fonts");
            }
            
        }
    }
}
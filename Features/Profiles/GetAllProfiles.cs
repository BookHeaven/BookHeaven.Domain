using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Profiles;

public static class GetAllProfiles
{
    public sealed record Query : IQuery<List<Profile>>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory)
        : IQueryHandler<Query, List<Profile>>
    {
        public async Task<Result<List<Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await context.Profiles.ToListAsync(cancellationToken);
        }
    }
}

using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Profiles;

public static class GetProfileById
{
    public sealed record Query(Guid Id) : IQuery<Profile>;
    
    internal sealed class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, Profile>
    {
        public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var profile = await context.Profiles.FirstOrDefaultAsync(p => p.ProfileId == request.Id, cancellationToken: cancellationToken);

            return profile != null ? profile : new Error("Profile not found");
        }
    }
}
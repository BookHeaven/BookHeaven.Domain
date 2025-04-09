using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Profiles;

public static class GetDefaultProfile
{
    public class Query : IQuery<Profile>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, Profile>
    {
        public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var profile = await context.Profiles.FirstOrDefaultAsync(p => p.Name == "Default", cancellationToken);

            return profile != null ? profile : new Error("Error", "Default profile not found");
        }
    }
}

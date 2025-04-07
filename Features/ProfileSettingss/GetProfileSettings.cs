using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHeaven.Domain.Features.ProfileSettingss;

public static class GetProfileSettings
{
    
    public sealed record Query(Guid ProfileId) : IQuery<ProfileSettings>;

    internal class QueryHandler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<QueryHandler> logger) : IQueryHandler<Query, ProfileSettings>
    {
        public async Task<Result<ProfileSettings>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var profileSettings = await context.ProfilesSettings
                .FirstOrDefaultAsync(ps => ps.ProfileId == request.ProfileId, cancellationToken);
            
            return profileSettings != null
                ? profileSettings
                : new Error("Error", "Profile settings not found");
        }
    }
}
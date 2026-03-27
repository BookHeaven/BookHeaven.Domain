using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Features.Profiles;

public static class GetProfileByName
{
    public sealed record Query(string Name) : ICustomQuery<Profile>;
    
    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICustomQueryHandler<Query, Profile>
    {
        public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var profile = await context.Profiles.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken: cancellationToken);
            
            return profile != null ? profile : new Error("Profile not found");
        }
    }
}
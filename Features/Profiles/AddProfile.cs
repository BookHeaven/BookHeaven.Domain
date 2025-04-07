using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Profiles;

public static class AddProfile
{
    public sealed record Command(Profile Profile) : ICommand<Profile>;
    
    internal class CommandHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Profile>
    {
        public async Task<Result<Profile>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            try
            {
                await context.Profiles.AddAsync(request.Profile, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error("Error", e.Message);
            }

            return request.Profile;
        }
    }
    
}
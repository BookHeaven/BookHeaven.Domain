using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.ProfileSettingss;

public static class UpdateProfileSettings
{
    public sealed record Command(ProfileSettings ProfileSettings) : ICommand;
    
    internal class CommandHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            context.ProfilesSettings.Update(request.ProfileSettings);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
    
}
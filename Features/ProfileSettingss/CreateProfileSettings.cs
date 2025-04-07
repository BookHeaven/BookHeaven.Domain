using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.ProfileSettingss;

public static class CreateProfileSettings
{
    public sealed record Command(ProfileSettings ProfileSettings) : ICommand;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            await context.ProfilesSettings.AddAsync(request.ProfileSettings, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
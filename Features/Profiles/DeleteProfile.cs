﻿using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Profiles;

public static class DeleteProfile
{
    public sealed record Command(Guid Id) : ICommand;
    
    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var profile = await context.Profiles.FirstOrDefaultAsync(p => p.ProfileId == request.Id, cancellationToken: cancellationToken);
            if (profile == null)
            {
                return new Error("Profile not found.");
            }

            context.Profiles.Remove(profile);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }

            return Result.Success();
        }
    }
}
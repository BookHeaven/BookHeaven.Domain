using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Profiles;

public static class CreateProfile
{
    public sealed record Command(string Name) : ICommand<Profile>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Profile>
    {

        public async Task<Result<Profile>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var profile = new Profile
            {
                Name = request.Name
            };
            
            await context.Profiles.AddAsync(profile, cancellationToken);
            
            var books = await context.Books
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            
            foreach (var book in books)
            {
                var progress = new BookProgress
                {
                    BookId = book.BookId,
                    ProfileId = profile.ProfileId,
                };
                await context.BooksProgress.AddAsync(progress, cancellationToken);
            }

            try 
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }
        
            return profile;
        }
    }
}
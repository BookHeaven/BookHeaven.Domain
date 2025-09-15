using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Extensions;

namespace BookHeaven.Domain.Features.Books;

public static class AddBook
{
    public sealed record Command(Book Book, string CoverPath, string EpubPath) : ICommand<Guid>;

    internal class CommandHandler(IDbContextFactory<DatabaseContext> dbContextFactory) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            
            await context.Books.AddAsync(request.Book, cancellationToken);
            
            if(request.Book.Author is not null && context.Authors.Any(a => a.AuthorId == request.Book.Author.AuthorId))
            {
                context.Entry(request.Book.Author).State = EntityState.Unchanged;
            }
            if(request.Book.Series is not null && context.Series.Any(s => s.SeriesId == request.Book.Series.SeriesId))
            {
                context.Entry(request.Book.Series).State = EntityState.Unchanged;
            }

            if (request.Book.Tags.Count > 0)
            {
                foreach (var tag in request.Book.Tags)
                {
                    var existingTag = await context.Tags.FirstOrDefaultAsync(t => t.TagId == tag.TagId, cancellationToken);
                    if (existingTag != null)
                    {
                        context.Entry(existingTag).State = EntityState.Unchanged;
                    }
                }
            }
            
            try 
            {
                await context.SaveChangesAsync(cancellationToken);
                await Utilities.StoreFile(request.CoverPath, request.Book.CoverPath(), cancellationToken);
                await Utilities.StoreFile(request.EpubPath, request.Book.EpubPath(), cancellationToken);
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }
            return request.Book.BookId;
        }
    }
}
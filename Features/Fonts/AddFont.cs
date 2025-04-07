using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHeaven.Domain.Features.Fonts;

public static class AddFont
{
    public sealed record Command(Font Font) : ICommand<Font>;
    
    internal class CommandHandler(
        IDbContextFactory<DatabaseContext> dbContextFactory,
        ILogger<CommandHandler> logger) : ICommandHandler<Command, Font>
    {
        public async Task<Result<Font>> Handle(Command request, CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            var font = new Font
            {
                Family = request.Font.Family,
                Style = request.Font.Style,
                Weight = request.Font.Weight,
                FileName = request.Font.FileName
            };

            context.Fonts.Add(font);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding font");
                return new Error("FONT_ADD_ERROR", ex.Message);
            }

            return font;
        }
    }
}
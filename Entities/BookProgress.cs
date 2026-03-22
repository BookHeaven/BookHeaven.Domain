using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHeaven.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookHeaven.Domain.Entities;

public partial class BookProgress
{
    public Guid BookProgressId { get; set; }

    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public int Chapter { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
    public int? PageCountPrev { get; set; }
    public int? PageCountNext { get; set; }
    public int BookWordCount { get; set; }
    public decimal Progress { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset? LastRead { get; set; }
    public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

}

internal class BookProgressConfig : IEntityTypeConfiguration<BookProgress>
{
    public void Configure(EntityTypeBuilder<BookProgress> builder)
    {
        builder.HasKey(bp => bp.BookProgressId);
        builder.Property(bp => bp.BookProgressId).ValueGeneratedOnAdd();
        
        builder
            .HasOne(bp => bp.Book)
            .WithMany(b => b.Progresses)
            .HasForeignKey(bp => bp.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(bp => bp.Profile)
            .WithMany(p => p.BooksProgress)
            .HasForeignKey(bp => bp.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
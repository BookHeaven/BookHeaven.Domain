using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHeaven.Domain.Extensions;

namespace BookHeaven.Domain.Entities;

public partial class BookProgress : EntityExtensions<BookProgress>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BookProgressId { get; set; }

    public Guid BookId { get; set; }
    [ForeignKey(nameof(BookId))]
    public Book? Book { get; set; }
    public Guid ProfileId { get; set; }
    [ForeignKey(nameof(ProfileId))]
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
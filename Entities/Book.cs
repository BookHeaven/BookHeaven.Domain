using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookHeaven.Domain.Extensions;
using BookHeaven.Domain.Helpers;

namespace BookHeaven.Domain.Entities;

public partial class Book : EntityExtensions<Book>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BookId { get; set; }
    public string? Title { get; set; }
    public Guid? AuthorId { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public Author? Author { get; set; }
    public Guid? SeriesId { get; set; }
    [ForeignKey(nameof(SeriesId))]
    public Series? Series { get; set; }
    public decimal? SeriesIndex { get; set; }
    public string? Publisher { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? Description { get; set; }
    public string? ISBN10 { get; set; }
    public string? ISBN13 { get; set; }
    public string? ASIN { get; set; }
    public string? UUID { get; set; }
    public string? Language { get; set; }

    [JsonIgnore]
    [InverseProperty(nameof(BookProgress.Book))]
    public virtual List<BookProgress> Progresses { get; set; } = [];
    
    public string EpubUrl => "/books/" + BookId + ".epub";

    public string FormattedFileName =>
        $"{(Author != null).Then($"{Author?.Name} - ")}{(Series != null).Then($"{Series?.Name} ({SeriesIndex?.ToString("0.##")}) - ")}{Title}.epub";
    
    public string? GetBookPath(string booksPath, bool checkPath = false)
    {
        var path = $"{booksPath}/{BookId}.epub";
        if (checkPath && !File.Exists(path))
        {
            return null;
        }
        return path;
    }
    
    public string CoverUrl => "/covers/" + BookId + ".jpg";
    
    public string? GetCoverPath(string coversPath, bool checkPath = false)
    {
        var path = $"{coversPath}/{BookId}.jpg";
        if (checkPath && !File.Exists(path))
        {
            return null;
        }
        return path;
    }

}
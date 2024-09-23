using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookHeaven.Domain.Extensions;

namespace BookHeaven.Domain.Entities;

public partial class Series : EntityExtensions<Series>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid SeriesId { get; set; }
    public string? Name { get; set; }
    [JsonIgnore]
    public List<Book> Books { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookHeaven.Domain.Extensions;

namespace BookHeaven.Domain.Entities;

public partial class Author : EntityExtensions<Author>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AuthorId { get; set; }
    public string? Name { get; set; }
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
    [JsonIgnore]
    public List<Book> Books { get; set; } = [];
}
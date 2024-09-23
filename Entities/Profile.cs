using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public partial class Profile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProfileId { get; set; }
    public string? Name { get; set; }
    public bool IsSelected { get; set; }
    [JsonIgnore]
    public ProfileSettings? ProfileSettings { get; set; }
    [JsonIgnore]
    public List<BookProgress> BooksProgress { get; set; } = [];
}
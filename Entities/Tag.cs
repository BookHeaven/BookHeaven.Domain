using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookHeaven.Domain.Entities;

public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public virtual List<Book> Books { get; set; }
}
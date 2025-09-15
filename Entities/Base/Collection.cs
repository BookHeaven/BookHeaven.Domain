using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHeaven.Domain.Enums;

namespace BookHeaven.Domain.Entities.Base;

public abstract class Collection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CollectionId { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public CollectionType CollectionType { get; set; }
    public Guid? ProfileId { get; set; }
    public int SortOrder { get; set; }
    public bool Pinned { get; set; }
}
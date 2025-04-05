using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Entities;

[PrimaryKey(nameof(Family), nameof(Style), nameof(Weight))]
public class Font
{
    public string Family { get; set; } = null!;
    public string Style { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string FileName { get; set; } = null!;

}
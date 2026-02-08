using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookHeaven.Domain.Entities;

public class ProfileSettings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProfileSettingsId { get; set; }

    public Guid ProfileId { get; set; }
    [JsonIgnore]
    [ForeignKey(nameof(ProfileId))]
    public Profile Profile { get; set; } = null!;
    
    public decimal FontSize { get; set; } = 16;
    public decimal LineHeight { get; set; } = 0;
    public decimal LetterSpacing { get; set; } = 0;
    public decimal WordSpacing { get; set; } = 0;
    public decimal ParagraphSpacing { get; set; } = 10;
    public decimal TextIndent { get; set; } = 1;
    public decimal HorizontalMargin { get; set; } = 3;
    public decimal VerticalMargin { get; set; } = 1;
    public decimal PageGap { get; set; } = 50;
    public int SelectedLayout { get; set; } = 0;
    public string SelectedFont { get; set; } = string.Empty;
}
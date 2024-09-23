using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookHeaven.Domain.Entities;

public partial class ProfileSettings : ObservableObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProfileSettingsId { get; set; }

    public Guid ProfileId { get; set; }
    [ForeignKey(nameof(ProfileId))]
    public Profile Profile { get; set; } = null!;

    [ObservableProperty] private double fontSize = 16;
    [ObservableProperty] private double lineHeight = 0;
    [ObservableProperty] private double letterSpacing = 0;
    [ObservableProperty] private double wordSpacing = 0;
    [ObservableProperty] private double paragraphSpacing = 10;
    [ObservableProperty] private double textIndent = 1;
    [ObservableProperty] private double horizontalMargin = 3;
    [ObservableProperty] private double verticalMargin = 1;
    [ObservableProperty] private int selectedLayout = 0;
}
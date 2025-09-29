namespace BookHeaven.Domain;

public static class Globals
{
    internal static string BooksPath { get; set; } = string.Empty;
    internal static string CoversPath { get; set; } = string.Empty;
    internal static string FontsPath { get; set; } = string.Empty;

    public static IReadOnlyList<string> SupportedFormats { get; } =
    [
        ".epub",
        ".pdf"
    ];
}
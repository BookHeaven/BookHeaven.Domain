using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Extensions;

public static class FontExtensions
{
    public static string Folder(this Font font, string basePath) => Path.Combine(basePath, font.Family);
    public static string File(this Font font, string basePath) => Path.Combine(basePath, font.Family, font.FileName);
    public static string Url(this Font font) => $"/fonts/{font.Family}/{font.FileName}";

    public static string GetFontFace(this Font font)
    {
        return $@"@font-face {{
            font-family: '{font.Family}';
            src: url('{font.Url()}');
            font-weight: {font.Weight};
            font-style: {font.Style};
        }}";
    }
}
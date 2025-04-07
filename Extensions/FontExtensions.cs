using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Extensions;

public static class FontExtensions
{
    public static string FolderPath(this Font font, string basePath) => Path.Combine(basePath, font.Family);
    public static string FilePath(this Font font, string basePath) => Path.Combine(basePath, font.Family, font.FileName);
    public static string Url(this Font font) => $"/fonts/{font.Family}/{font.FileName}";

    public static string GetFontFace(this Font font, string? basePath = null)
    {
        return $@"@font-face {{
            font-family: '{font.Family}';
            src: url('{(!string.IsNullOrEmpty(basePath) ? font.GetBase64Src(basePath) : font.Url())}');
            {(font.Weight != "normal" && font.Weight != "all" ? $"font-weight: {font.Weight};" : string.Empty)}
            {(font.Style != "normal" && font.Style != "all" ? $"font-style: {font.Style};" : string.Empty)}
        }}";
    }

    private static string GetBase64Src(this Font font, string basePath)
    {
        if (string.IsNullOrEmpty(font.FileName))
        {
            return string.Empty;
        }
        if (!File.Exists(font.FilePath(basePath)))
        {
            return string.Empty;
        }
        var fileBytes = File.ReadAllBytes(font.FilePath(basePath));
        return $"data:font/{font.FileName.Split(".")[1]};base64,{Convert.ToBase64String(fileBytes)}";
    }
}
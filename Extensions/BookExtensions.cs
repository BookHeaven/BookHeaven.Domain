using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Enums;

namespace BookHeaven.Domain.Extensions;

public static class BookExtensions
{
    public static BookProgress Progress(this Book book) => book.Progresses.First();
    public static BookStatus ReadingStatus(this Book book)
    {
        if (book.Progress().EndDate != null)
        {
            return BookStatus.Finished;
        }
        
        if (book.Progress().ElapsedTime != TimeSpan.Zero)
        {
            return BookStatus.Reading;
        }

        return BookStatus.New;
    }
    public static List<Book> GetReadingBooks(this IEnumerable<Book> books) => books.Where(b => b.ReadingStatus() == BookStatus.Reading).ToList();
    public static bool AnyReading(this IEnumerable<Book> books) => books.Any(b => b.ReadingStatus() == BookStatus.Reading);

    public static string GetCoverAsBase64(this Book book, string coversPath)
	{
		var path = book.GetCoverPath(coversPath);
		if (path == null || !File.Exists(path))
		{
			return string.Empty;
		}
		return "data:image/jpeg;base64," + Convert.ToBase64String(File.ReadAllBytes(path));
	}
}
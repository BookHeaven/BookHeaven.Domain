using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Enums;
using BookHeaven.Domain.Helpers;

namespace BookHeaven.Domain.Extensions;

public static class BookExtensions
{
	public static string FormattedFileName(this Book book) => $"{(book.Author != null).Then($"{book.Author?.Name} - ")}{(book.Series != null).Then($"{book.Series?.Name} ({book.SeriesIndex?.ToString("0.##")}) - ")}{book.Title}.epub";
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
    
    public static string EpubUrl(this Book book) => "/books/" + book.BookId + ".epub";
    public static string CoverUrl(this Book book) => "/covers/" + book.BookId + ".jpg";
    
    public static string EpubPath(this Book book, string booksPath) => Path.Combine(booksPath, $"{book.BookId}.epub");
    public static string CoverPath(this Book book, string coversPath) => Path.Combine(coversPath, $"{book.BookId}.jpg");

    public static string GetCoverAsBase64(this Book book, string coversPath)
	{
		var path = book.CoverPath(coversPath);
		if (!File.Exists(path))
		{
			return string.Empty;
		}
		return "data:image/jpeg;base64," + Convert.ToBase64String(File.ReadAllBytes(path));
	}

	
}
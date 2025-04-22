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
    
    public static int GetCountByStatus(this List<Book> books, BookStatus status) => books.Count(b => b.ReadingStatus() == status);
    
    public static string EpubUrl(this Book book) => "/books/" + book.BookId + ".epub";
    public static string CoverUrl(this Book book) => "/covers/" + book.BookId + ".jpg";
    
    public static string EpubPath(this Book book) => Path.Combine(Globals.BooksPath, $"{book.BookId}.epub");
    public static string CoverPath(this Book book) => Path.Combine(Globals.CoversPath, $"{book.BookId}.jpg");

    public static string GetCoverAsBase64(this Book book)
	{
		var path = book.CoverPath();
		if (!File.Exists(path))
		{
			return string.Empty;
		}
		return "data:image/jpeg;base64," + Convert.ToBase64String(File.ReadAllBytes(path));
	}

	public static void UpdateFrom(this Book book, Book updatedBook)
	{
		book.Title = updatedBook.Title;
		book.AuthorId = updatedBook.AuthorId;
		book.SeriesId = updatedBook.SeriesId;
		book.SeriesIndex = updatedBook.SeriesIndex;
		book.Publisher = updatedBook.Publisher;
		book.PublishedDate = updatedBook.PublishedDate;
		book.Description = updatedBook.Description;
		book.ISBN10 = updatedBook.ISBN10;
		book.ISBN13 = updatedBook.ISBN13;
		book.ASIN = updatedBook.ASIN;
		book.UUID = updatedBook.UUID;
		book.Language = updatedBook.Language;
	}

	
}
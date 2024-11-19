using BookHeaven.Domain.Entities;

namespace BookHeaven.Domain.Extensions;

public static class BookExtensions
{
    public static BookProgress? Progress(this Book book) => book.Progresses.FirstOrDefault();
    public static bool IsStartedReading(this Book book) => book.Progress()?.ElapsedTime != TimeSpan.Zero;
    public static bool IsFinishedReading(this Book book) => book.Progress()?.EndDate != null;
    public static List<Book> GetReadingBooks(this IEnumerable<Book> books) => books.Where(b => b.IsStartedReading() && !b.IsFinishedReading()).ToList();
    public static bool AnyReading(this IEnumerable<Book> books) => books.Any(b => b.IsStartedReading() && !b.IsFinishedReading());
}
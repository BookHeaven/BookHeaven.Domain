using BookHeaven.Domain.Abstractions;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Enums;
using BookHeaven.Domain.Extensions;
using BookHeaven.Domain.Features.Books;
using BookHeaven.Domain.Features.BooksProgress;
using BookHeaven.Domain.Localization;
using BookHeaven.Domain.Shared;
using MediatR;

namespace BookHeaven.Domain.Services;

public class BookManager(
    IAlertService alertService,
    ISender sender)
{
    private List<Book> _books = [];
    public List<Book> Books => Filter == BookStatus.All ? _books : _books.Where(b => b.ReadingStatus() == Filter).ToList();
    public bool IsEmpty => _books.Count == 0;
    public int CountByStatus(BookStatus status) => _books.GetCountByStatus(status);
    
    public BookStatus Filter { get; set; } = BookStatus.All;

    /*private async Task ClearCache(Book book, bool showToast = true)
    {
        var progress = book.GetCachePath(CacheKey.Progress);
        var styles = book.GetCachePath(CacheKey.Styles);

        if (File.Exists(progress))
        {
            File.Delete(progress);
        }

        if (File.Exists(styles))
        {
            File.Delete(styles);
        }

        if (showToast) await alertService.ShowToast("Cache cleared");
    }*/
    
    public async Task GetBooksAsync(Guid profileId)
    {
        var getBooks = await sender.Send(new GetAllBooks.Query(profileId));
        if (getBooks.IsSuccess)
        {
            _books = getBooks.Value;
        }
    }

    public async Task AppendBookAsync(Guid profileId, Guid bookId)
    {
        var getBook = await sender.Send(new GetBook.Query(bookId));
        if(getBook.IsFailure) return;
        
        var getProgress = await sender.Send(new GetBookProgressByProfile.Query(bookId,profileId));
        if(getProgress.IsFailure) return;
        
        var book = getBook.Value;
        book.Progresses.Add(getProgress.Value);
        _books.Add(book);
    }
    
    public async Task DeleteBookAsync(Book book)
    {
        var epubPath = book.EpubPath();
        var coverPath = book.CoverPath();

        try
        {
            if (File.Exists(epubPath))
            {
                File.Delete(epubPath);
            }

            if (File.Exists(coverPath))
            {
                File.Delete(coverPath);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete book files", ex);
        }
        
        var deleteBook = await sender.Send(new DeleteBook.Command(book.BookId));
        if (deleteBook.IsFailure) throw new Exception(deleteBook.Error.Description);
        
        if(_books.Count > 0) _books.Remove(book);
    }
    
    public async Task MarkAsNewAsync(Book book)
    {
        var result = await alertService.ShowConfirmationAsync("Are you sure?", "This will reset your progress, which can't be undone unless you delete the book and download it again.");
        if (!result) return;
        
        var progress = book.Progress();
        progress.StartDate = null;
        progress.EndDate = null;
        progress.Progress = 0;
        progress.ElapsedTime = TimeSpan.Zero;
        progress.LastRead = null;
        progress.Chapter = 0;
        progress.Page = 0;
        progress.PageCount = 0;
        progress.PageCountNext = 0;
        progress.PageCountPrev = 0;
        await sender.Send(new UpdateBookProgress.Command(progress));
        //await ClearCache(book, false);
        await alertService.ShowToastAsync(Translations.BOOK_MARKED_AS_NEW);
    }
    
    public async Task MarkAsFinishedAsync(Book book)
    {
        var progress = book.Progress();
        if(progress.ElapsedTime == TimeSpan.Zero)
        {
            progress.StartDate = DateTimeOffset.Now;
        }
        progress.EndDate = DateTimeOffset.Now;
        progress.Progress = 100;
        await sender.Send(new UpdateBookProgress.Command(progress));
        //await ClearCache(book, false);
        await alertService.ShowToastAsync(Translations.BOOK_MARKED_AS_FINISHED);
    }
}
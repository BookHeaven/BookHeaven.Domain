namespace BookHeaven.Domain.Helpers;

public static class Helpers
{
	/// <summary>
	/// Prettier null coalescing operator when there's no else statement.
	/// </summary>
	public static T? Then<T>(this bool value, T result)
	{
		return value ? result : default;
	}
	
	public static string? GetBookPath(string booksPath, Guid bookId, bool checkPath = false)
	{
		var path = $"{booksPath}/{bookId}.epub";
		if (checkPath && !File.Exists(path))
		{
			return null;
		}
		return path;
	}
	
	public static string? GetCoverPath(string coversPath, Guid bookId, bool checkPath = false)
	{
		var path = $"{coversPath}/{bookId}.jpg";
		if (checkPath && !File.Exists(path))
		{
			return null;
		}
		return path;
	}
}
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using SReader.Console.Models;
    using Microsoft.Data.Sqlite;

    namespace SReader.Console.Services;

public class BookmarkManager
{
    private static List<Bookmark> GetAllBookmarks()
    {
        if (!File.Exists("bookmarks.json"))
        {
            return new List<Bookmark>(); // якщо файлу немає, то повертаєтьс порожній список
        }

        string json = File.ReadAllText("bookmarks.json");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Bookmark>(); // перевірка якщо файл пустий
        }

        return JsonSerializer.Deserialize<List<Bookmark>>(json) ?? new List<Bookmark>();
    }

    public static Bookmark GetBookmark(string bookPath)
    {
        var allBookmarks = GetAllBookmarks();
        return allBookmarks.FirstOrDefault(bookmark => bookmark.BookPath == bookPath);
    }

    public static void SaveBookmark(Bookmark bookmarkToSave)
    {
        var allBookmarks = GetAllBookmarks();
        var existingBookmark = allBookmarks.FirstOrDefault(b => b.BookPath == bookmarkToSave.BookPath);
        if (existingBookmark != null)
        {
            existingBookmark.SymbolIndex = bookmarkToSave.SymbolIndex;
        }
        else
        {
            allBookmarks.Add(bookmarkToSave);
        }

        string json = JsonSerializer.Serialize(allBookmarks);
        File.WriteAllText("bookmarks.json", json);
    }
}
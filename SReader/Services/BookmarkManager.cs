using System.Text.Json;
using SReader.Models;
using System.IO;
namespace SReader.Services;

public class BookmarkManager
{
    public static string SerializeBookmark(Bookmark bookmark)
    {
        string json = JsonSerializer.Serialize(bookmark);
        File.WriteAllText("bookmarks.json", json);
        return json;
    }

    public static Bookmark DeSerializeBookmark()
    {
        string json = File.ReadAllText("bookmarks.json");
        return JsonSerializer.Deserialize<Bookmark>(json);
    }
}
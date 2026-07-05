using System.Text.Json;
using SReader.Models;
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
        if (!File.Exists("bookmarks.json"))
        {
            return null;
        }
        string json = File.ReadAllText("bookmarks.json");
        return JsonSerializer.Deserialize<Bookmark>(json);
    }
}
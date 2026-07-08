namespace SReader.Console;
using VersOne.Epub;
using System.Text.RegularExpressions;
using System.Net;
using Fb2.Document;
using System.Linq;
using System.Text;

public class BookParser
{ 
    public static string ReadAnyBook(string filepath)
{
    if (filepath.EndsWith(".txt"))
        return ReadTxt(filepath);
    else if (filepath.EndsWith(".epub"))
        return ReadEpubBook(filepath);
    else if  (filepath.EndsWith(".fb2"))
        return ReadFb2Book(filepath);
    else
    {
        return "This format is not supported";
    }
}

private static string ReadTxt(string filepath)
{
    string fileText = File.ReadAllText(filepath);
    return fileText;
}

private static string ReadEpubBook(string filepath)
{
    EpubBook book = EpubReader.ReadBook(filepath);
    System.Console.WriteLine($"Title: {book.Title},  Author: {book.Author}\n");

    StringBuilder fullBookText = new StringBuilder();
    
    foreach (var chapter in book.ReadingOrder)
    {
        // вирізання всіх HTML тегів
        string cleanText = Regex.Replace(chapter.Content, "<.*?>", string.Empty);
        // перетворення веб-символів на звичайні пробіли та лапки
        cleanText = WebUtility.HtmlDecode(cleanText).Trim();
        //якщо після очищення це не порожня сторінка
        if (!string.IsNullOrWhiteSpace(cleanText))
        {
            fullBookText.AppendLine(cleanText); // додавання тексту цього розділу до загального
            fullBookText.AppendLine(); // додавання відступів між рядками
        }
    }
    return fullBookText.ToString();
}

private static string ReadFb2Book(string filepath)
{
    var fb2Document = new Fb2Document();

    using (var stream = File.OpenRead(filepath))
    {
        fb2Document.Load(stream); // завантаження stream в контейнер
    }

    if (fb2Document.Title != null)
    {
        // в fb2 заголовок може містити форматування, тому потрібно перетворити все в чистий рядок
        var titleText = fb2Document.Title.ToString() ?? "The title is not specified";
    }

    var mainBody = fb2Document.Bodies?.FirstOrDefault(); // перший елемент в fb2 це сам текст
    
    if (mainBody != null)
    {
        return mainBody.ToString();
    }
    return "No text chapters were found in the book";
}
}
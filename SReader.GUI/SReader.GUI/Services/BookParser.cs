using System.IO;
using System.Net.Http;
using Avalonia;
using VersOne.Epub;
using System.Text.RegularExpressions;
using System.Net;
using Fb2.Document;
using System.Linq;
using System.Text;

namespace SReader.Console;


public class BookParser
{
    public static string ReadAnyBook(Stream fileStream, string fileName)
    {
        string lowerName = fileName.ToLower();
        
        if (fileName.EndsWith(".txt"))
            return ReadTxt(fileStream);
        else if (fileName.EndsWith(".epub"))
            return ReadEpubBook(fileStream);
        else if (fileName.EndsWith(".fb2") || lowerName.EndsWith(".fb2.zip"))
            return ReadFb2Book(fileStream);
        else
        {
            return "This format is not supported";
        }
    }

    private static string ReadTxt(Stream fileStream)
    {
        using (StreamReader streamReader = new StreamReader(fileStream))
        {
            return streamReader.ReadToEnd();
        }
    }

    private static string ReadEpubBook(Stream fileStream)
    {
        EpubBook book = EpubReader.ReadBook(fileStream);
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

    private static string ReadFb2Book(Stream fileStream)
    {
        var fb2Document = new Fb2Document();

        fb2Document.Load(fileStream); // завантаження stream в контейнер

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
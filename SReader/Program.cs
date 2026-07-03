using VersOne.Epub;
using System.Text.RegularExpressions;
using System.Net;

Console.Write("Enter the path to the book : ");

string inputPath = Console.ReadLine();

inputPath = inputPath.Trim('\'', '"');

string bookText = ReadAnyBook(inputPath);
Console.WriteLine(bookText);
string ReadAnyBook(string filepath)

{
    if (filepath.EndsWith(".txt"))
        return File.ReadAllText(filepath);
    
    else if (filepath.EndsWith(".epub"))
        return ReadEpubBook(filepath);

    else
    {
        return "This format is not supported";
    }
}

string ReadEpubBook(string filepath)
{
    EpubBook book = EpubReader.ReadBook(filepath);
    Console.WriteLine($"Title: {book.Title},  Author: {book.Author}\n");

    foreach (var chapter in book.ReadingOrder)
    {
        // вирізання всіх HTML тегів
        string cleanText = Regex.Replace(chapter.Content, "<.*?>", string.Empty);
        // перетворення веб-символів на звичайні пробіли та лапки
        cleanText = WebUtility.HtmlDecode(cleanText).Trim();
        //якщо після очищення є більше чим 200 символів, то це точно розділ а не обкладинка
        if (cleanText.Length > 200)
        {
            return cleanText;
        }
    }
    return "No text chapters were found in the book";
}


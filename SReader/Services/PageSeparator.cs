using SReader.Models;

namespace SReader.Services;

public class PageSeparator
{
    public static void Separator(string bookText, string bookPath)
    {
        int pageSize = 1500;
        int currentPosInText = 0;

        Bookmark savedBookmark = BookmarkManager.GetBookmark(bookPath);

        if (savedBookmark != null && savedBookmark.BookPath == bookPath)
        {
            currentPosInText = savedBookmark.SymbolIndex;
        }

        while (true)
        {
            if (currentPosInText >= bookText.Length)
            {
                Console.WriteLine("The book is over");
            }
            
            int charsToTake = Math.Min(pageSize, bookText.Length - currentPosInText);

            // if (currentPosInText + charsToTake > bookText.Length)  // цей блок не потрібен (дублює int charsToTake)
            // {
            //     Console.WriteLine("The book has ended");
            //     break;
            // }

            Console.Clear();
            string page =  bookText.Substring(currentPosInText, charsToTake);
            Console.WriteLine(page);
            
            Console.WriteLine("\n[Press any key to go to the next page, or Q to exit]");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Q)
            {
                Bookmark myBookmark = new Bookmark();
                myBookmark.BookPath = bookPath;
                myBookmark.SymbolIndex = currentPosInText; // на якому символі зупинились
                
                BookmarkManager.SaveBookmark(myBookmark); // запис у файл bookmarks.json
                
                break;
            } 
            
            currentPosInText += charsToTake;
        }
    }
}
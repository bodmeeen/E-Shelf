using System;
using SReader.Console.Models;

namespace SReader.Console.Services;

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
        
        System.Console.WriteLine("Press any key to continue from the bookmark, or R to start over");
        var keyStart = System.Console.ReadKey().Key;
        if (keyStart == ConsoleKey.R)
        {
            var bookIsOver = BookmarkManager.GetBookmark(bookPath);
            bookIsOver.SymbolIndex = 0;
            BookmarkManager.SaveBookmark(bookIsOver);
            currentPosInText = 0;
        }

        while (true)
        {
            if (currentPosInText >= bookText.Length)
            {
                System.Console.WriteLine("The book is over! Press Q to exit or Y to start over");
                var key2  = System.Console.ReadKey().Key;
                if (key2 == ConsoleKey.Y)
                {
                    var bookIsOver = BookmarkManager.GetBookmark(bookPath);
                    bookIsOver.SymbolIndex = 0;
                    BookmarkManager.SaveBookmark(bookIsOver);
                    currentPosInText = 0;
                }
                else
                {
                    break;
                }
            }
            
            int charsToTake = Math.Min(pageSize, bookText.Length - currentPosInText);

            System.Console.Clear();
            string page =  bookText.Substring(currentPosInText, charsToTake);
            System.Console.WriteLine(page);
            
            System.Console.WriteLine("\n[Press any key to go to the next page, or Q to exit]");
            var key = System.Console.ReadKey().Key;
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
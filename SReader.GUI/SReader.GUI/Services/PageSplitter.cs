using System;
using System.Collections.Generic;

namespace SReader.GUI.Services;

public static class PageSplitter
{
    public static List<string[]> SplitIntoPages(string[] paragraphs, double screenHeight, double fontSize = 16)
    {
        var pages = new List<string[]>();
        var currentPage = new List<string>();
        
        double lineHeight = fontSize * 1.5; // вирахування висоти рядка
        int maxLines = (int)(screenHeight * 0.85 / lineHeight); // *0.85 для компенсації відступів margin між абзацами
        if (maxLines < 10) maxLines = 10;

        int currentLines = 0;
        int charsPerLine = 40;

        foreach (var paragraph in paragraphs)
        {
            if (string.IsNullOrWhiteSpace(paragraph)) continue;
            
            int paragraphLines = (int)Math.Ceiling((double)paragraph.Length / charsPerLine) + 1;

            if (currentLines + paragraphLines > maxLines) // перевірка на перевовнення екрана
            {
                if (currentPage.Count > 0)
                {
                    pages.Add(currentPage.ToArray());
                    currentPage.Clear();
                    currentLines = 0;
                }
            }
            currentPage.Add(paragraph);
            currentLines +=  charsPerLine;
        }

        if (currentPage.Count > 0)
        {
            pages.Add(currentPage.ToArray());
        }
        return pages;
    }
}
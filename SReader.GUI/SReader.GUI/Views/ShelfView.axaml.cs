using System;
using System.IO;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SReader.Console;

namespace SReader.GUI.Views;

public partial class ShelfView : UserControl
{
    public event Action<string[]>? BookOpened;  // створення події яка може передавати string[]
        
    public ShelfView()
    {
        InitializeComponent();
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    // використання модифікатора async, оскільки відкриття провідника потребує часу на вибір файлу
    private async void OnOpenBookClicked(object sender, RoutedEventArgs e)
    {   
        // відкриття рідного провідника системи (на Linux або Android)
        var topLevel = TopLevel.GetTopLevel(this);
        // фільтр для книг, щоб андроїд розумів формати
        var bookFileType = new FilePickerFileType("Книги (FB2, EPUB, TXT)")
        {
            Patterns = new[] { "*.fb2", "*.txt", "*.epub", "*.fb2.zip" }, // патерни закінчень файлів для Linux/Windows
            MimeTypes = new[]
            {
                "application/epub+zip",
                "text/plain",               // .txt
                "application/zip",          // .fb2
                "text/html",                // альтернатива XML
                "application/x-fictionbook+xml", // офіційний тип FB2
                "application/octet-stream", // дозволяє відкривати файли які android бачить як .bin
            }
        };
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Оберіть файл книги",
            AllowMultiple = false,
            FileTypeFilter = new []{ bookFileType, FilePickerFileTypes.All }
        });
    
        if (files.Count >= 1)
        {
            try
            {
                var selectedFile = files[0];
                await using var fileStream = await selectedFile.OpenReadAsync();
                string fileName = selectedFile.Name;  // передача в BookParser 
                string text = BookParser.ReadAnyBook(fileStream, fileName);
                // розбиття суцільного тексту на окремі абзаци
                var paragraphs = text.Split(new[] { "\r\n\r\n", "\n\n", "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
                BookOpened?.Invoke(paragraphs); // замість BookListBox надсилається сигнал що книга відкрита
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ПОМИЛКА: {ex.Message}");
            }
        }
    }
}
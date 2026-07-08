using System;
using System.IO;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SReader.Console;
using SReader.Console.Services;

namespace SReader.GUI.Views;

public partial class MainView : UserControl
{
    public MainView()
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
                "application/zip",
                "text/plain",               // .txt
                "application/zip",          // .fb2
                "text/html",                // альтернатива XML
                "application/x-fictionbook+xml", // офіційний тип FB2
                "application/octet-stream", // дозволяє відкривати файли які android бачить як .bin
                "application/octet-stream"
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

                // отримання потоку даних від системи
                await using var fileStream = await selectedFile.OpenReadAsync();

                // створення тимчасового шляху у внутрішній пам'яті додатку
                string tempFilePath = Path.Combine(Path.GetTempPath(), selectedFile.Name);

                // копіювання книги в тимчасовий файл (щоб обійти обмеження Scoped Storage)
                await using (var tempFileStream = File.Create(tempFilePath))
                {
                    await fileStream.CopyToAsync(tempFileStream);
                }

                // тепер передаємо чистий та безпечний шлях парсеру
                string text = BookParser.ReadAnyBook(tempFilePath);
                var bookmark = BookmarkManager.GetBookmark(tempFilePath);

                // розбиття суцільного тексту на окремі абзаци
                var paragraphs = text.Split(new[] { "\r\n\r\n", "\n\n", "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                // передавання масиву абзаців у список замість єдиного TextBlock
                BookListBox.ItemsSource = paragraphs;
            }
            catch (Exception ex)
            {
                BookListBox.ItemsSource = new[] { $"ПОМИЛКА ВІДКРИТТЯ ФАЙЛУ:\n{ex.Message}\n\nДеталі:\n{ex.StackTrace}" };
            }
        }
    }
}
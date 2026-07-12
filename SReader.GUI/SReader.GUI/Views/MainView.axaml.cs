using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace SReader.GUI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        ShowShelf();
    }

    private void ShowShelf()
    {
        var shelfView = new ShelfView();
        // коли в ShelfView спрацює BookOpened?.Invoke(paragraphs) то виконати
        shelfView.BookOpened += (paragraphs) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var readerView = new ReaderView(paragraphs); // створення читалки та перенесення її в змінну

                readerView.BackRequested += () =>
                {
                    ShowShelf();
                };
                ScreenContainer.Content = readerView;
            });
        };
        ScreenContainer.Content = shelfView;
    }
}
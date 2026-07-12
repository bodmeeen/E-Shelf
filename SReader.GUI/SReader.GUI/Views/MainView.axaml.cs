using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
            ScreenContainer.Content = new ReaderView(paragraphs);
        };
        ScreenContainer.Content = shelfView;
    }
}
using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;

namespace SReader.GUI.Views;

public partial class ReaderView : UserControl
{
    public event Action? BackRequested;
    
    public ReaderView()
    {
        InitializeComponent();
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public ReaderView(string[] paragraphs) : this()  // : this() викликає конструктор вище
    {
        BookListBox.ItemsSource = paragraphs;
    }

    private void BackToMainScreen(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke();
    }

    // змінення прозорості кнопки назад (при читанні) при кліку на екран
    private void OnTextTapped(object sender, TappedEventArgs e)
    {
        BackButton.IsVisible = !BackButton.IsVisible;
    }
}
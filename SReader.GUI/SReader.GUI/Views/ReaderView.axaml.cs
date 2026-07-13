using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using SReader.GUI.Services;

namespace SReader.GUI.Views;

public partial class ReaderView : UserControl
{
    public event Action? BackRequested;
    
    private string[] _allParagraphs =  Array.Empty<string>();
    private List<string[]> _pages = new();
    private int _currentPage = 0;
    
    public ReaderView()
    {
        InitializeComponent();
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    
    public ReaderView(string[] paragraphs) : this()  // : this() викликає конструктор вище
    {
        BookListBox.ItemsSource = paragraphs;
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        double textAreaHeight = BookListBox.Bounds.Height; // передавання в карусель усіх сторінок разом
        double textAreaWidth = BookListBox.Bounds.Width;
        
    }
    void CalculatePages(double textAreaHeight)
    {
        _pages = PageSplitter.SplitIntoPages(_allParagraphs, textAreaHeight);
        BookListBox.ItemsSource = _pages;
        BookListBox.SelectedIndex = 0;
    }


    private void BackToMainScreen(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke();
    }

    // змінення прозорості кнопки назад (при читанні) при кліку на екран
    private void OnTextTapped(object sender, TappedEventArgs e)
    {
        // double tapX = e.GetPosition(this).X; // куди тапнули
        // double screenWidth  = this.Bounds.Width;
        //
        // if (tapX < screenWidth * 0.25) // якщо тапнули в ліву частину екрану
        // {
        //     PageCarousel.Previous();
        // }
        // else if (tapX > screenWidth * 0.75) // якщо тапнули в праву частину екрану
        // {
        //     PageCarousel.Next();
        // }
        // else // якщо тапнули посередині
        // {
        BackButton.IsVisible = !BackButton.IsVisible;
        // }
    }
}
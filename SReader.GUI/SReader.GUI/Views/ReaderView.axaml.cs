using System.Text;
using Avalonia.Controls;
using Fb2.Document.Models;

namespace SReader.GUI.Views;

public partial class ReaderView : UserControl
{
    public ReaderView()
    {
        InitializeComponent();
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public ReaderView(string[] paragraphs) : this()  // : this() викликає конструктор вище
    {
        BookListBox.ItemsSource = paragraphs;
    }
}
using System;
using Avalonia.Controls;

namespace SReader.GUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        
        var screen = Screens.ScreenFromVisual(this) ?? Screens.Primary;

        if (screen != null)
        {
            // фізичний розмір екрана
            double screenWidth = screen.Bounds.Width;
            double screenHeight = screen.Bounds.Height;

            // масштабування (DPI) операційної системи
            double scaling = screen.Scaling;

            // робоча область без урахування системних панелей
            double workAreaWidth = screen.WorkingArea.Width;
            double workAreaHeight = screen.WorkingArea.Height;
            
            App.ScreenHeight = screenHeight;
            App.ScreenWidth = screenWidth;
        }
    }
}
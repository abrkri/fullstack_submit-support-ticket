using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MyApp.Client.ViewModels;

namespace MyApp.Client.WinUI;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Root.DataContext = App.Services.GetRequiredService<StudentReportsViewModel>();
    }
}
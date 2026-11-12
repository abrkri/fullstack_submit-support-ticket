using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Client.ViewModels;

namespace MyApp.Client.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<StudentReportsViewModel>();
    }
}

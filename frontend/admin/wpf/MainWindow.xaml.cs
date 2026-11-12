using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Client.ViewModels;

namespace MyApp.Admin.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<AdminReportsViewModel>();
    }
}

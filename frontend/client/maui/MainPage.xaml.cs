using MyApp.Client.ViewModels;

namespace MyApp.Client.Maui;

public partial class MainPage : ContentPage
{
    public MainPage(StudentReportsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

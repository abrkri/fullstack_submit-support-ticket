using MyApp.Client.ViewModels;

namespace MyApp.Admin.Maui;

public partial class MainPage : ContentPage
{
    public MainPage(AdminReportsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MyApp.Client.Services;
using MyApp.Client.ViewModels;

namespace MyApp.Admin.WinUI;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;
    private Window? window;

    public App()
    {
        InitializeComponent();
        var services = new ServiceCollection();
        services.AddSingleton(new HttpClient { BaseAddress = new Uri("http://localhost:5222/") });
        services.AddSingleton<IAdminApiService, AdminApiService>();
        services.AddSingleton<AdminReportsViewModel>();
        Services = services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // XAML resources are available after the App constructor has completed.
        Resources["AdminViewModel"] = Services.GetRequiredService<AdminReportsViewModel>();
        window = new MainWindow();
        window.Activate();
    }
}
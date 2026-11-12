using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Client.Services;
using MyApp.Client.ViewModels;

namespace MyApp.Admin.Wpf;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;

    public App()
    {
        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5222/")
        });
        services.AddSingleton<IAdminApiService, AdminApiService>();
        services.AddTransient<AdminReportsViewModel>();

        Services = services.BuildServiceProvider();
    }
}

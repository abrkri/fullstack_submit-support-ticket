using Microsoft.Extensions.Logging;
using MyApp.Client.Services;
using MyApp.Client.ViewModels;

namespace MyApp.Client.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        const string apiBaseAddress = "http://localhost:5222/";

        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri(apiBaseAddress)
        });
        builder.Services.AddSingleton<IStudentApiService, StudentApiService>();
        builder.Services.AddTransient<StudentReportsViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

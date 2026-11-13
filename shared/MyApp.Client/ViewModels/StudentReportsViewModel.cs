using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.Client.Services;
using MyApp.Shared.Models;
namespace MyApp.Client.ViewModels;
public partial class StudentReportsViewModel(IStudentApiService studentApiService) : ObservableObject
{
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string location = string.Empty;
    [ObservableProperty] private DateTime dueDate = DateTime.Now.AddDays(1);
    [ObservableProperty] private ReportCategories category = new ReportCategories();
    [ObservableProperty] private int affectedDeviceCount = 0;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private bool hasError;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitReportCommand))]
    private bool isBusy;
    private bool CanSubmit() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitReportAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            HasError = false;
            StatusMessage = "Hibabejelentés beküldése...";
            var created = await studentApiService.CreateReportAsync(
                new CreateReportRequest
                {
                    Description = Description,
                    Location = Location,
                    DueDate = DueDate,
                    AffectedDeviceCount = AffectedDeviceCount,
                    Category = Category
                }
            );
            Description = string.Empty;
            Location = string.Empty;
            DueDate = DateTime.Now.AddDays(1);
            AffectedDeviceCount = 0;
            Category = new ReportCategories();
            StatusMessage = $"Sikeres hibabejelentés. Azonosító: {created.Id}.";
        }
        catch (ApiException exception) { HasError = true; StatusMessage = exception.Message; }
        catch (Exception exception) { HasError = true; StatusMessage = $"Nem sikerült kapcsolódni a backendhez: {exception.Message}"; }
        finally { IsBusy = false; }
    }
}

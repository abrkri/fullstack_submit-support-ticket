using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.Client.Services;
using MyApp.Shared.Models;
namespace MyApp.Client.ViewModels;
public partial class AdminReportsViewModel(IAdminApiService adminApiService) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<ReportResponse> reports = [];
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private bool hasError;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadReportsCommand))]
    [NotifyCanExecuteChangedFor(nameof(CloseReportCommand))]
    private bool isBusy;
    private bool CanLoad() => !IsBusy;
    private bool CanClose(ReportResponse? report) => !IsBusy && report is { IsClosed: false };

    [RelayCommand(CanExecute = nameof(CanLoad))]
    private async Task LoadReportsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            HasError = false;
            StatusMessage = "Hibabejelentések betöltése...";
            Reports = new ObservableCollection<ReportResponse>(await adminApiService.GetReportsAsync());
            StatusMessage = $"{Reports.Count} hibabejelentés betöltve.";
        }
        catch (ApiException exception) { HasError = true; StatusMessage = exception.Message; }
        catch (Exception exception) { HasError = true; StatusMessage = $"Nem sikerült kapcsolódni a backendhez: {exception.Message}"; }
        finally { IsBusy = false; }
    }

    [RelayCommand(CanExecute = nameof(CanClose))]
    private async Task CloseReportAsync(ReportResponse? report)
    {
        if (!CanClose(report)) return;
        try
        {
            IsBusy = true;
            HasError = false;
            StatusMessage = "Hibabejelentés lezárása...";
            var closed = await adminApiService.CloseReportAsync(report!.Id);
            var previous = Reports.FirstOrDefault(r => r.Id == closed.Id);
            if (previous is not null) Reports[Reports.IndexOf(previous)] = closed;
            StatusMessage = $"A(z) {closed.Id}. hibabejelentés lezárva.";
        }
        catch (ApiException exception) { HasError = true; StatusMessage = exception.Message; }
        catch (Exception exception) { HasError = true; StatusMessage = $"Nem sikerült kapcsolódni a backendhez: {exception.Message}"; }
        finally { IsBusy = false; }
    }
}

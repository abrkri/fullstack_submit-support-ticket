using MyApp.Shared.Models;
namespace MyApp.Client.Services;
public interface IAdminApiService
{
    Task<List<ReportResponse>> GetReportsAsync();
    Task<ReportResponse> CloseReportAsync(int id);
}

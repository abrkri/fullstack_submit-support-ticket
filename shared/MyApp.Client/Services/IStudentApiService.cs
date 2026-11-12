using MyApp.Shared.Models;
namespace MyApp.Client.Services;
public interface IStudentApiService
{
    Task<ReportResponse> CreateReportAsync(CreateReportRequest request);
}

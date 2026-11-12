using MyApp.Shared.Models;
using MyApp.WebApi.Results;
namespace MyApp.WebApi.Services;
public interface IReportService
{
    Task<Result<ReportResponse>> CreateAsync(CreateReportRequest request);
    Task<List<ReportResponse>> GetAllAsync();
    Task<Result<ReportResponse>> CloseAsync(int id);
}

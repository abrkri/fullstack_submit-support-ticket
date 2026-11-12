using System.Net.Http.Json;
using MyApp.Shared.Models;
namespace MyApp.Client.Services;
public class StudentApiService(HttpClient httpClient) : IStudentApiService
{
    public async Task<ReportResponse> CreateReportAsync(CreateReportRequest request)
    {
        using var response = await httpClient.PostAsJsonAsync("reports", request);
        if (!response.IsSuccessStatusCode)
            throw await ApiException.FromResponseAsync(response, "A hibabejelentés beküldése nem sikerült.");
        return await response.Content.ReadFromJsonAsync<ReportResponse>()
            ?? throw new InvalidOperationException("A backend üres választ adott.");
    }
}

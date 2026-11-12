using System.Net.Http.Json;
using MyApp.Shared.Models;
namespace MyApp.Client.Services;
public class AdminApiService(HttpClient httpClient) : IAdminApiService
{
    public async Task<List<ReportResponse>> GetReportsAsync()
    {
        using var response = await httpClient.GetAsync("reports");
        if (!response.IsSuccessStatusCode)
            throw await ApiException.FromResponseAsync(response, "A hibabejelentések betöltése nem sikerült.");
        return await response.Content.ReadFromJsonAsync<List<ReportResponse>>() ?? [];
    }
    public async Task<ReportResponse> CloseReportAsync(int id)
    {
        using var response = await httpClient.PostAsync($"reports/{id}/close", null);
        if (!response.IsSuccessStatusCode)
            throw await ApiException.FromResponseAsync(response, "A hibabejelentés lezárása nem sikerült.");
        return await response.Content.ReadFromJsonAsync<ReportResponse>()
            ?? throw new InvalidOperationException("A backend üres választ adott.");
    }
}

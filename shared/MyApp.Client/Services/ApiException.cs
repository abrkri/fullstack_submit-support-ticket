using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MyApp.Shared.Models;

namespace MyApp.Client.Services;

public class ApiException : Exception
{
    public ApiException(HttpStatusCode statusCode, string? code, string message)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode StatusCode { get; }

    public string? Code { get; }

    public static async Task<ApiException> FromResponseAsync(
        HttpResponseMessage response,
        string fallbackMessage)
    {
        ApiProblemDetailsResponse? problem = null;

        try
        {
            problem = await response.Content.ReadFromJsonAsync<ApiProblemDetailsResponse>();
        }
        catch (JsonException)
        {
            // A fallback message is used when the response is not ProblemDetails JSON.
        }

        return new ApiException(
            response.StatusCode,
            problem?.Code,
            problem?.Detail ?? fallbackMessage);
    }
}

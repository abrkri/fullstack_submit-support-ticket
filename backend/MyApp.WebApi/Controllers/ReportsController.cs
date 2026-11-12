using Microsoft.AspNetCore.Mvc;
using MyApp.Shared.Models;
using MyApp.WebApi.Results;
using MyApp.WebApi.Services;
namespace MyApp.WebApi.Controllers;
[ApiController]
[Route("reports")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateReportRequest request) =>
        ToActionResult(await reportService.CreateAsync(request), StatusCodes.Status201Created);

    [HttpGet]
    public async Task<ActionResult<List<ReportResponse>>> GetAll() => Ok(await reportService.GetAllAsync());

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id) => ToActionResult(await reportService.CloseAsync(id));

    private IActionResult ToActionResult(Result<ReportResponse> result, int successCode = StatusCodes.Status200OK)
    {
        if (result.IsSuccess) return StatusCode(successCode, result.Value);
        var status = result.ErrorCode == "report_not_found" ? 404 : 400;
        var problem = new ProblemDetails
        {
            Type = "about:blank", Title = status == 404 ? "Not Found" : "Bad Request",
            Status = status, Detail = result.ErrorMessage
        };
        problem.Extensions["code"] = result.ErrorCode;
        return StatusCode(status, problem);
    }
}

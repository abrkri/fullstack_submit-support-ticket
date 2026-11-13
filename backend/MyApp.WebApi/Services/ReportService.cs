using Microsoft.EntityFrameworkCore;
using MyApp.Shared.Models;
using MyApp.WebApi.Data;
using MyApp.WebApi.Models;
using MyApp.WebApi.Results;
namespace MyApp.WebApi.Services;
public class ReportService(AppDbContext dbContext) : IReportService
{
    public async Task<Result<ReportResponse>> CreateAsync(CreateReportRequest request)
    {
        var description = request.Description?.Trim() ?? string.Empty;
        var location = request.Location?.Trim() ?? string.Empty;
        var duedate = request.DueDate;
        var affectedDeviceCount = request.AffectedDeviceCount;
        var category = request.Category;
        if (description.Length == 0)
            return Result<ReportResponse>.Failure("description_required", "A hiba leírása kötelező.");
        if (description.Length > 1000)
            return Result<ReportResponse>.Failure("description_too_long", "A hiba leírása legfeljebb 1000 karakter lehet.");
        if (location.Length == 0)
            return Result<ReportResponse>.Failure("location_required", "A helyszín megnevezése kötelező.");
        if (location.Length > 100)
            return Result<ReportResponse>.Failure("location_too_long", "A helyszín megnevezése legfeljebb 100 karakter lehet.");
        if (duedate < DateTime.Now.AddDays(1))
            return Result<ReportResponse>.Failure("duedate_invalid", "A határidő nem lehet a jelenlegi dátumnál korábbi.");
        if (affectedDeviceCount < 1 || affectedDeviceCount > 999)
            return Result<ReportResponse>.Failure("affecteddevicecount_invalid", "Az érintett eszközök száma nem haladhatja meg az 1000-et és nem lehet 1-nél kevesebb");
        if (category is null)
            return Result<ReportResponse>.Failure("category_required", "A kategória megadása kötelező.");
        var report = new Report {
            Description = description,
            IsClosed = false,
            Location = location,
            DueDate = duedate,
            AffectedDeviceCount = affectedDeviceCount,
            Category = category
        };
        dbContext.Reports.Add(report);
        await dbContext.SaveChangesAsync();
        return Result<ReportResponse>.Success(ToResponse(report));
    }
    public async Task<List<ReportResponse>> GetAllAsync()
    {
        var reports = await dbContext.Reports.AsNoTracking().OrderByDescending(r => r.Id).ToListAsync();
        return reports.Select(ToResponse).ToList();
    }
    public async Task<Result<ReportResponse>> CloseAsync(int id)
    {
        var report = await dbContext.Reports.SingleOrDefaultAsync(r => r.Id == id);
        if (report is null)
            return Result<ReportResponse>.Failure("report_not_found", "A hibabejelentés nem található.");
        // Ismételt lezáráskor is ugyanaz a lezárt állapot az eredmény.
        report.IsClosed = true;
        await dbContext.SaveChangesAsync();
        return Result<ReportResponse>.Success(ToResponse(report));
    }
    private static ReportResponse ToResponse(Report report) => new()
    {
        Id = report.Id,
        Description = report.Description,
        IsClosed = report.IsClosed,
        Location = report.Location,
        DueDate = report.DueDate,
        AffectedDeviceCount = report.AffectedDeviceCount,
        Category = report.Category
    };
}

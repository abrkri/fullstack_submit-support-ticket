using MyApp.Shared.Models;

namespace MyApp.WebApi.Models;
public class Report
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsClosed { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
    public int AffectedDeviceCount { get; set; } = 0;
    public ReportCategories Category { get; set; } = new ReportCategories();


}

namespace Reports.Api.Models;

[ExcludeFromCodeCoverage]
public class Report {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsReportingService { get; set; } = false;
    public bool IsCrystalReport { get; set; } = false;
    public string ReportId { get; set; }
    
    public IDictionary<string, ReportParam> Params { get; set; }
}

using Reports.Api.Repositories;
using WebTest.Abstractions;
using WebTest.Models;

namespace Reports.Api.Controllers;
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "reportsAuthorization")]
public class ReportsController : Controller {
    private readonly IConfiguration _config;
    private readonly ReportsRepository _reportsRepository;
    private readonly TokenService _tokenService;
    public ReportsController(IConfiguration config,
        IHttpContextAccessor context
        ) {
        _config = config;
        _reportsRepository = new ReportsRepository(config);
        _tokenService = new TokenService(context);
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ArrayResult<Report>))]
    public async Task<IActionResult> GetReports() {
        var requestClaims = Request.HttpContext.User.Claims.ToList();
        var userClaims = new UserClaims
        {
            RoleName = requestClaims.SingleOrDefault(c => c.Type == "rolename")?.Value
        };

        var reports = await _reportsRepository.GetReports(userClaims.RoleName);
       
        AddReportingServiceReports(reports);
        AddDownloadCrystalReports(reports);
        return Ok(new ArrayResult<Report> { Result = reports });
    }
    private void AddReportingServiceReports(IEnumerable<Report> reports) {
        var dwhReports = reports?.Where(r => r.IsReportingService);
        if (dwhReports is null) return;
        var requestClaims = Request.HttpContext.User.Claims.ToList();
        var userClaims = new UserClaims {
                RoleName = requestClaims.SingleOrDefault(c => c.Type == "rolename")?.Value
            };
        var reportingServiceUrl = _config.GetValue<string>("ReportingServiceUrl");
        foreach (var report in dwhReports) {
            var claims = new Dictionary<string, object> {
                { "reportName", report.ReportId },
                { "roleName", userClaims.RoleName },
            };
            var dwhToken = _tokenService.CreateToken(claims);
            report.Url = $"{reportingServiceUrl}?token={dwhToken}";
        }
    }

    private void AddDownloadCrystalReports(IEnumerable<Report> reports) {
        var borderouReports = reports.Where(r => r.IsCrystalReport);
        if (borderouReports is null || !borderouReports.Any()) return;
        var requestClaims = Request.HttpContext.User.Claims.ToList();
        var userClaims = new UserClaims
        {
            RoleName = requestClaims.SingleOrDefault(c => c.Type == "rolename")?.Value
        };
        var downloadCrystalReportUrl = _config.GetValue<string>("CrystalReportsUrl");
        foreach (var report in borderouReports) {
            var claims = new Dictionary<string, object> {
                { "reportName", report.ReportId },
                { "roleName", userClaims.RoleName }
            };
            var crystalReportToken = _tokenService.CreateToken(claims);
            report.Url = $"{downloadCrystalReportUrl}?token={crystalReportToken}&startDate={{startDate}}&endDate={{endDate}}";
        }


    }
}

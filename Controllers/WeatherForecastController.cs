using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;

namespace ReportViewerCore_Test.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("pdf")]
    public ActionResult GetPdf()
    {
        string fileName = "ReceiptV1.rdlc";
        string filePath = "./ReportFiles/";
  
        filePath = Path.Combine(filePath, fileName);
        Console.WriteLine($"File Path is {filePath}");

        using(var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            LocalReport report = new LocalReport();
            report.LoadReportDefinition(fileStream);
            report.SetParameters(new[] 
            { 
                new ReportParameter("MembershipNo", "AUD0001"),
                new ReportParameter("FullName", "Muhammed"),
                new ReportParameter("State", "Abu Dhabi"),
                new ReportParameter("District", "Kasargod"),
                new ReportParameter("Mandalam", "Trikaripur"),
                new ReportParameter("Panchayath", "Trikaripur"),
                new ReportParameter("MembershipDate", "22/05/2022"),
                new ReportParameter("CollectedBy", "Abdul Hameed"),
                new ReportParameter("Area", "Musaffah")
            });

            byte[] pdf = report.Render("PDF");
            return File(pdf, "application/pdf", "Receipt.pdf");
        }
    }
}

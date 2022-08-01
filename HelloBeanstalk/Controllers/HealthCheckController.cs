using Microsoft.AspNetCore.Mvc;

namespace HelloBeanstalk.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "WeatherForecast service OK";
    }
}
using Microsoft.AspNetCore.Mvc;

namespace HelloBeanstalk.Controllers;

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
    
    [Route("summary")]
    [HttpGet()]
    public string Summary()
    {
        var random = new Random();
        return random.Next(3) switch
        {
            0 => "It's going to be a HOT one, folks! We're looking at temperatures in the high 90s.",
            1 => "Better bundle up, it's going to be COLD outside. Expect freezing temperatures throughout the day.",
            _ => "We're in for some nice, balmy weather. This is a great day to go to the beach!"
        };
    }

}

//ec2 -> target group -> health check -> path
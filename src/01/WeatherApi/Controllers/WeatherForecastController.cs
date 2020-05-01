using System;
using Microsoft.AspNetCore.Mvc;

namespace CrappyWeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static int _requestCounter = 0;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public string Get()
        {
            if (++_requestCounter % 3 == 0)
            {
                throw new Exception();
            }

            var rng = new Random();
            return Summaries[rng.Next(Summaries.Length)];
        }
    }
}

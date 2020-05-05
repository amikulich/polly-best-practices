using System;
using System.Threading;
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
        public string Get(int id)
        {
            //if (++_requestCounter % 3 == 0)
            //{
            //    Thread.Sleep(TimeSpan.FromSeconds(10));
            //    //throw new Exception();
            //}

            Thread.Sleep(TimeSpan.FromSeconds(5));

            return Summaries[id];
        }
    }
}

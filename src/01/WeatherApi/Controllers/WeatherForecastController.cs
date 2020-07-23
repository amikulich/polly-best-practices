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

            #region Case 1

            if (++_requestCounter % 3 == 0)
            {
                throw new NotImplementedException();
            }

            return Summaries[0];

            #endregion

            #region Case 2

            //Thread.Sleep(TimeSpan.FromSeconds(5));
            //return Summaries[1];

            #endregion

            #region Case 3

            //throw new NotImplementedException();

            #endregion

            #region Case 4

            //Thread.Sleep(TimeSpan.FromSeconds(3));

            //return Summaries[id];

            #endregion
        }

        #region Case 3

        [HttpPost]
        public string Post()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

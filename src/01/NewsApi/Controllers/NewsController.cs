using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsApi.Models;

namespace NewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly CrapyWeatherApiClient _apiClient;

        public NewsController(CrapyWeatherApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult<News>> Get()
        {
            var news = new News()
            {
                WorldNews = "Hello World!",
                WeatherForecast = await _apiClient.Get()
            };

            return Ok(news);
        }
    }
}

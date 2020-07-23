using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsApi
{
    public class CrapyWeatherApiClient
    {
        private readonly HttpClient _httpClient;

        public CrapyWeatherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(int id)
        {
            var response = await _httpClient.GetAsync($"weatherforecast?id={id}");

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString;
            }

            throw new Exception(responseString);
        }

        public async Task<string> Post()
        {
            var response = await _httpClient.PostAsync("weatherforecast", null);

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString;
            }

            throw new Exception(responseString);
        }
    }
}

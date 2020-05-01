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

        public async Task<string> Get()
        {
            var response = await _httpClient.GetAsync("weatherforecast");

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString;
            }

            throw new Exception(responseString);
        }
    }
}

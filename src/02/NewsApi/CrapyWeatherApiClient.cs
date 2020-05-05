using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Caching;
using Polly.Registry;

namespace NewsApi
{
    public class CrapyWeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IPolicyRegistry<string> _policyRegistry;

        public CrapyWeatherApiClient(HttpClient httpClient, IPolicyRegistry<string> policyRegistry)
        {
            _httpClient = httpClient;
            _policyRegistry = policyRegistry;
        }

        public async Task<string> Get(int id)
        {
            var policy = _policyRegistry.Get<AsyncCachePolicy<HttpResponseMessage>>("cache");

            var executionContext = new Context($"id-{id}");
            var response = 
                await policy.ExecuteAsync(async (x) => await _httpClient.GetAsync($"weatherforecast?id={id}"), executionContext);

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString;
            }

            throw new Exception(responseString);
        }
    }
}

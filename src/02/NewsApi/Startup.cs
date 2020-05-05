using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using Polly.Timeout;

namespace NewsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();
            services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();

            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            IAsyncPolicy<HttpResponseMessage> retry = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(3));

            IAsyncPolicy<HttpResponseMessage> timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

            registry.Add("retryPolicy", retry);

            services.AddHttpClient<CrapyWeatherApiClient>((client) =>
            {
                client.BaseAddress = new Uri("http://localhost:61720");
            }); //.AddPolicyHandler(retry.WrapAsync(timeoutPolicy));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IPolicyRegistry<string> policyRegistry, IAsyncCacheProvider memoryCache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Func<Context, HttpResponseMessage, Ttl> ttlFilter = (context, result) =>
                new Ttl(result.IsSuccessStatusCode ? TimeSpan.FromSeconds(30) : TimeSpan.Zero);

            AsyncCachePolicy<HttpResponseMessage> policy = 
                Policy.CacheAsync(memoryCache.AsyncFor<HttpResponseMessage>(), 
                    new ResultTtl<HttpResponseMessage>(ttlFilter) );

            policyRegistry.Add("cache", policy);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

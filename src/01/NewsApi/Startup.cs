using System;
using System.Net.Http;
using System.Threading.Tasks;
using EasyExtensions.Polly.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            #region Initial 

            services.AddHttpClient<CrapyWeatherApiClient>((client) =>
            {
                client.BaseAddress = new Uri("http://localhost:61720");
            });

            #endregion

            #region Case 1: Simple Retry

            //IAsyncPolicy<HttpResponseMessage> policy = Policy
            //    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            //    .RetryAsync(3);
            ////.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i*3));

            //services.AddHttpClient<CrapyWeatherApiClient>((client) =>
            //    {
            //        client.BaseAddress = new Uri("http://localhost:61720");
            //    })
            //    .AddPolicyHandler(policy);

            #endregion

            #region Case 2: Retry + Handle Timeouts

            //IAsyncPolicy<HttpResponseMessage> retry = Policy
            //    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            //    .Or<TimeoutRejectedException>()
            //    .RetryAsync(3);

            //var timeoutPolicy = Policy.TimeoutAsync(timeout: TimeSpan.FromSeconds(3), onTimeoutAsync: (ctx, t, task, e) =>
            //{
            //    Console.Write(e.Message);
            //    return Task.CompletedTask;
            //});

            //var policy = retry.WrapAsync(timeoutPolicy);

            //services.AddHttpClient<CrapyWeatherApiClient>((client) =>
            //    {
            //        client.BaseAddress = new Uri("http://localhost:61720");
            //    })
            //    .AddPolicyHandler(policy);

            #endregion

            #region Case 3: Dynamic policies

            //var registry = services.AddPolicyRegistry();

            //IAsyncPolicy<HttpResponseMessage> policy = Policy
            //    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            //    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(attempt));

            //registry.Add("RetryPolicy", policy);

            //services.AddHttpClient<CrapyWeatherApiClient>(client =>
            //    {
            //        client.BaseAddress = new Uri("http://localhost:61720");
            //    })
            //    .AddPolicyHandlerFromRegistry((policyRegistry, request) =>
            //    {
            //        if (request.Method == HttpMethod.Get)
            //        {
            //            return registry.Get<IAsyncPolicy<HttpResponseMessage>>("RetryPolicy");
            //        }

            //        return Policy.NoOpAsync<HttpResponseMessage>();
            //    });

            #endregion

            #region Case 4: Caching

            //services.AddMemoryCache();
            //services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
            //services.AddContextSetters();
            //services.AddPolicyRegistry();

            //services.AddHttpClient<CrapyWeatherApiClient>()
            //    .ConfigureHttpClient(client =>
            //    {
            //        client.BaseAddress = new Uri("http://localhost:61720");
            //    })
            //    .CachePerUriAndMethod()
            //    .AddPolicyHandlerFromRegistry((registry, request) =>
            //        registry.GetCachePolicyFor<CrapyWeatherApiClient>());

            #endregion
        }

        #region Cases 1-3

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion

        #region Case 4: Caching

        //public void Configure(IApplicationBuilder app,
        //    IWebHostEnvironment env,
        //    IPolicyRegistry<string> policyRegistry,
        //    IAsyncCacheProvider cacheProvider)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    policyRegistry.AddCachePolicyFor<CrapyWeatherApiClient>(cacheProvider, TimeSpan.FromSeconds(60));

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}

        #endregion
    }
}

using BLOG.Application.Caching;
using BLOG.Application.Common.Behaviours;
using BLOG.Infrastructure.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BLOG.Api.Setups
{
    public static class CacheSetup
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

            services.AddMemoryCache();
            services.AddScoped<ICacheStore, MemoryCacheStore>();

            return services;
        }
    }
}

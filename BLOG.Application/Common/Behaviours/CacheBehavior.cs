using BLOG.Application.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLOG.Application.Common.Behaviours
{
    public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICacheStore _cache;
        private readonly CacheConfiguration _cacheConfiguration;
        private MemoryCacheEntryOptions _cacheOptions;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public CacheBehavior(ICacheStore cacheStore, IOptions<CacheConfiguration> options)
        {
            _cache = cacheStore;
            _cacheConfiguration = options.Value;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(options.Value.SlidingExpiration))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(options.Value.AbsoluteExpiration));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // polecenie czyszczące bufory
            if (request is ICacheCleanCommand command)
            {
                TResponse response = await next();
                _cache.GetKeys().ToList().ForEach(key =>
                {
                    if (key.StartsWith($"{command.CacheGroup}_"))
                        _cache.Remove(key);
                });
                return response;
            }

            // zapytanie z bufora
            if (request is ICacheableQuery query)
            {
                TResponse response;

                // ominięcie bufora
                if (query.BypassCache)
                    return await next();

                var key = $"{query.CacheGroup}_{query.CacheKey}";

                // wyszukanie wyniku w buforze
                if (_cache.TryGet(key, out TResponse cachedResponse) && cachedResponse != null)
                {
                    response = cachedResponse;
                }
                else
                {
                    // nie znaleziono
                    try
                    {
                        // blokowanie pamięci podręcznej 
                        await semaphore.WaitAsync();

                        if (_cache.TryGet(key, out TResponse cachedResponse2) && cachedResponse2 != null)
                        {
                            // inne zapytanie przed zablokowaniem dodało wynik do bufora
                            response = cachedResponse2;
                        }
                        else
                        {
                            // nie znaleziono - wykonanie zapytania
                            response = await next();

                            // dodanie do bufora
                            _cache.Add(response, key, _cacheOptions);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }

                return response;
            }
            else
            {
                return await next();
            }
        }
    }
}

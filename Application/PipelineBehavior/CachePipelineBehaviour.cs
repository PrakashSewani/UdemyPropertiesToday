using Application.Models;
using Application.PipelineBehavior.Contracts;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PipelineBehavior
{
    public class CachePipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICacheable
    {
        private readonly IDistributedCache _cache;
        private readonly CacheSetting _cacheSetting;

        public CachePipelineBehaviour(IDistributedCache cache, IOptions<CacheSetting> cacheSetting)
        {
            _cache = cache;
            _cacheSetting = cacheSetting.Value;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.BypassCache) return await next();

            TResponse response;
            string cacheKey = $"{_cacheSetting.ApplicationName}:{request.CacheKey}";
            var cacheResponse = await _cache.GetAsync(cacheKey, cancellationToken);

            if (cacheResponse != null)
            {
                if (request.ValueModified)
                {
                    await _cache.RemoveAsync(cacheKey, cancellationToken);
                    response = await next();
                }
                else
                {
                    response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cacheResponse));
                }
            }
            else
            {
                response = await GetResponseAndWriteToCache();
            }

            return response;

            async Task<TResponse> GetResponseAndWriteToCache()
            {
                response = await next();
                if (response != null)
                {
                    var slidingExpiration = request.SlidingExpiration == null ?
                        TimeSpan.FromMinutes(_cacheSetting.SlidingExpiration)
                        : request.SlidingExpiration;

                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = slidingExpiration,
                        AbsoluteExpiration = DateTime.Now.AddDays(1)
                    };

                    var serializedData = Encoding.Default
                        .GetBytes(
                        JsonConvert
                        .SerializeObject(response,
                        Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        }));

                    await _cache.SetAsync(cacheKey, serializedData, cacheOptions);
                }
                return response;
            }
        }
    }
}

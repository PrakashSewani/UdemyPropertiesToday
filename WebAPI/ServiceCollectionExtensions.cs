using Application.Models;

namespace WebAPI
{
    public static class ServiceCollectionExtensions
    {
        public static CacheSetting GetCacheSettings(this IServiceCollection services, IConfiguration config)
        {
            var cacheSettingConfigurations = config.GetSection("CacheSettings");

            services.Configure<CacheSetting>(cacheSettingConfigurations);

            return cacheSettingConfigurations.Get<CacheSetting>();
        }
    }
}

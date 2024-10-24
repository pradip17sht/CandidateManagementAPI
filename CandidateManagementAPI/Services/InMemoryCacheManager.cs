using Microsoft.Extensions.Caching.Memory;

namespace CandidateManagementAPI.Services
{
    public class InMemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, object value)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
        }

        public T Get<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }
    }
}

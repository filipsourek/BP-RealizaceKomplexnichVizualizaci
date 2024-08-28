using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace SmartBuildingVisualization.Client.Core.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;
        private int _itemCount = 0;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

        }

        /// <summary>
        /// Získá hodnotu z cache na základě zadaného klíče.
        /// </summary>
        /// <typeparam name="T">Typ hodnoty, kterou chcete získat z cache.</typeparam>
        /// <param name="key">Klíč, pod kterým je hodnota uložena v cache.</param>
        /// <returns>Hodnota z cache pro zadaný klíč.</returns>
        public T Get<T>(string key)
        {
            if(_memoryCache.TryGetValue(key, out T value)) 
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Uloží hodnotu do cache s určeným klíčem a nastavitelnou dobou vypršení platnosti.
        /// </summary>
        /// <typeparam name="T">Typ hodnoty, kterou chcete uložit do cache.</typeparam>
        /// <param name="key">Klíč, pod kterým bude hodnota uložena v cache.</param>
        /// <param name="value">Hodnota, kterou chcete uložit do cache.</param>
        /// <param name="expirationMinutes">Doba vypršení platnosti v minutách (výchozí je 30 minut).</param>
        public void Set<T>(string key, T value, int expirationMinutes = 30)
        {
            if (_itemCount == 500)
            {
                ClearCache(0.1);
            }
            var options = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(expirationMinutes))
                            .SetSize(1)
                            .RegisterPostEvictionCallback((key, value, reason, state) =>
                            {
                                _itemCount--;
                            });
            _memoryCache.Set(key, value, options);
            _itemCount++;
        }

        /// <summary>
        /// Vymaže veškerou paměťovou cache.
        /// </summary>
        public Task ClearCache(double value = 1.0)
        {
            var memCache = _memoryCache as MemoryCache;
            if (memCache is not null)
            {
                memCache.Compact(value);
            }

            return Task.CompletedTask;
        }
    }
}

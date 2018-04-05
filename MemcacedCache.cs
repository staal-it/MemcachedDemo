using System;
using System.Net;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace MemcachedTryout
{
   internal class MemcacedCache : ICache
   {
      private readonly MemcachedClient _cache;
      private readonly ILogger _logger;

      public MemcacedCache()
      {
         _logger = new Logger();
         var config = new MemcachedClientConfiguration();
         config.Servers.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211));
         config.Protocol = MemcachedProtocol.Binary;
         
         _cache = new MemcachedClient(null, config);

         _cache.NodeFailed += NodeFailedEvent;
      }

      public async Task<bool> AddAsync<T>(string key, T value)
      {
         _logger.Debug("Adding item to cache with key {0}", key);

         return await _cache.StoreAsync(StoreMode.Set, key, value, TimeSpan.FromDays(90));
      }

      public async Task<T> GetWithSetAsync<T>(string cacheKey, Func<Task<T>> initializeFunction)
      {
         var data = await GetAsync<T>(cacheKey);

         if (data != null)
         {
            return data;
         }

         data = await initializeFunction();
         await AddAsync(cacheKey, data);

         return data;
      }

      public async Task<T> GetAsync<T>(string key)
      {
         var cacheItem = await _cache.GetAsync<T>(key);

         if (cacheItem == null)
         {
            _logger.Debug("Cache miss with key {0}", key);
            return default(T);
         }

         _logger.Debug("Cache hit with key {0}", key);

         return cacheItem;
      }

      public async Task<bool> RemoveAsync(string key)
      {
         _logger.Debug("Removing item from cache with key {0}", key);

         return await _cache.RemoveAsync(key);
      }

      private void NodeFailedEvent(IMemcachedNode memcachedNode)
      {
         _logger.Error("Node unavailable");
      }
   }
}
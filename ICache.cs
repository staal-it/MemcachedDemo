using System;
using System.Threading.Tasks;

namespace MemcachedTryout
{
   public interface ICache
   {
      Task<bool> AddAsync<T>(string key, T value);

      Task<T> GetAsync<T>(string key);

      Task<T> GetWithSetAsync<T>(string cacheKey, Func<Task<T>> initializeFunction);

      Task<bool> RemoveAsync(string key);
   }
}
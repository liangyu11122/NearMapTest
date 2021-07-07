using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace NMTest.DataSource
{
	public class DistributedCacheStore : IDistributedCacheStore, IDisposable
	{
		private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
		public T GetObjectFromCache<T>(string cacheItemName)
		{
			Thread.Sleep(50);
			var typedResult = default(T);
			try
			{
				var cacheObj = _cache.Get(cacheItemName);
				if (cacheObj != null)
				{
					typedResult = (T)cacheObj;
				}
				return (T)typedResult;
			}
			catch (InvalidCastException ex)
			{
				Console.WriteLine("Unable cast to specific format Details: {0}", ex);
				throw;
			}
		}

		public T SetCachedObject<T>(string cacheItemName, int cacheTimeInMinutes, Func<T> objectSettingFunction)
		{
			var cachedObject = objectSettingFunction();
			var type = cachedObject.GetType();
			if (type.IsGenericType && type == typeof(Dictionary<string, string>))
			{
				if (((ICollection)cachedObject).Count > 0)
				{
					_cache.Set(cacheItemName, cachedObject, DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes));
				}
			}
			else
			{
				_cache.Set(cacheItemName, cachedObject, DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes));
			}

			return (T)cachedObject;
		}

		public object Get(string key)
		{
			return _cache.Get(key);
		}

		public object Set(string key, int cacheTimeInMinutes, object item)
		{
			return _cache.Set(key, item, DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes));
		}
		public void Remove(string key)
		{
			_cache.Remove(key);
		}
		public void Dispose()
		{
			_cache?.Dispose();

		}
	}
}
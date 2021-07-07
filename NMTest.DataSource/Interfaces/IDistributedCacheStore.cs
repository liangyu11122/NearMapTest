using System;

namespace NMTest.DataSource
{
    public interface IDistributedCacheStore
    {
        T GetObjectFromCache<T>(string cacheItemName);
        T SetCachedObject<T>(string cacheItemName, int cacheTimeInMinutes, Func<T> objectSettingFunction);
    }
}

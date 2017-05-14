namespace BikeShopWebApi.Cache
{
    /// <summary>
    /// This is a shim around a static class so we don't have to take a depenency on the specific static
    /// implementation and can more effectively unit test our classes that rely on this abstraction.
    /// </summary>
    public class CacheAdapter : ICache
    {
        public T Get<T>(string key)
        {
            return Cache.Get<T>(key);
        }

        public void Set<T>(T input, string key, int expirationTimeInMinutes)
        {
         Cache.Set(input, key, expirationTimeInMinutes);
        }
    }
}
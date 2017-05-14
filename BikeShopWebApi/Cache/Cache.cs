using System;
using System.Runtime.Caching;

namespace BikeShopWebApi.Cache
{
    /// <summary>
    /// Our in memory cache. Constructed as a static class to demonstrate how to effectively 
    /// unit test and break the dependency on static classes in your designs.
    /// </summary>
    public static class Cache
    {
        private static MemoryCache InternalCache { get; }

        static Cache()
        {
            InternalCache = new MemoryCache(nameof(BikeShopWebApi));
        }

        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return (T)InternalCache.Get(key);

        }

        public static void Set<T>(T input, string key, int expirationTimeInMinutes)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            InternalCache.AddOrGetExisting(key, input,
                new CacheItemPolicy() {SlidingExpiration = TimeSpan.FromMinutes(expirationTimeInMinutes)});
        }
    }
}
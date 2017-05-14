namespace BikeShopWebApi.Cache
{
    public interface ICache
    {
        T Get<T>(string key);

        void Set<T>(T input, string key, int expirationTimeInMinutes);

    }
}
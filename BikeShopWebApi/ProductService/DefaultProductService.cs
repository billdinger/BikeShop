using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using BikeShopWebApi.Cache;
using BikeShopWebApi.ProductService.Exceptions;
using BikeShopWebApi.ProductService.Models;
using Newtonsoft.Json;

namespace BikeShopWebApi.ProductService
{
    /// <summary>
    /// This class is used to demonstrate how to mock and test things such as an HTTP client, and a "soap service"
    /// that doesn't exist.
    /// </summary>
    public class DefaultProductService : IProductService, IDisposable
    {
        private const string AllProductsCacheKey = "AllProducts";
        private const int SearchCacheExpirationTimeInMinutes = 15;

        private ICache Cache { get; }

        private HttpClient Client { get; }

        public DefaultProductService(HttpMessageHandler handler, ICache cache)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            Cache = cache;
            Client = new HttpClient(handler);
        }


        public IList<Product> GetAllProducts()
        {
            // Retrieve from Cache if possible.
            var cacheItem = Cache.Get<IList<Product>>(AllProductsCacheKey);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            // construct our HTTP Request.
            var response =  Client.GetAsync("https://localhost:1701/SomeOldSoapService").Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ProductErrorException($"The response returned from our SoapService Get all products was an error! " +
                                                $"Received the status code {response.StatusCode} with content of {response.Content}");
            }
            if (response.Content == null)
            {
                throw new NoProductsFoundException("No products found!");
            }


            // deserialize
            var soapResult =  response.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<IList<Product>>(soapResult);

            // add to our cache.
            Cache.Set(products, AllProductsCacheKey, Int32.MaxValue);

            return products;
        }

        public IList<Product> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            // see if query exists in our cache.
            // Retrieve from Cache if possible.
            var cacheItem = Cache.Get<IList<Product>>(query);
            if (cacheItem != null)
            {
                return cacheItem;
            }


            // construct our HTTP Request.
            var response =Client.GetAsync($"https://localhost:1701/SoapSearchService?searchTerm={query}&maxResult=50").Result;
            if (response == null)
            {
                return new List<Product>();
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new SearchErrorException( $"The response returned from our SoapService for search was an error! " +
                                                $"Received the status code {response.StatusCode} with content of {response.Content}");
            }

            // deserialize
            var soapResult = response.Content.ReadAsStringAsync().Result;
            var searchResult = JsonConvert.DeserializeObject<IList<Product>>(soapResult);

            // add to our cache.
            Cache.Set(searchResult, query, SearchCacheExpirationTimeInMinutes);

            return searchResult;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Client?.Dispose();
            }
            _disposed = true;

        }
    }
}
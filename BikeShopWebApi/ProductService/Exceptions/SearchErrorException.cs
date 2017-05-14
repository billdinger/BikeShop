using System;

namespace BikeShopWebApi.ProductService.Exceptions
{
    public class SearchErrorException : Exception
    {
        public SearchErrorException(string message) : base(message) { }
    }
}
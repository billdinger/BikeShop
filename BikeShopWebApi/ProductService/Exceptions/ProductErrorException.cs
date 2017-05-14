using System;

namespace BikeShopWebApi.ProductService.Exceptions
{
    public class ProductErrorException : Exception
    {
        public ProductErrorException(string message) : base(message) { }
    }
}
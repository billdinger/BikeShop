using System;

namespace BikeShopWebApi.ProductService.Exceptions
{
    public class NoProductsFoundException : Exception
    {
        public NoProductsFoundException(string message) : base(message) { }
    }
}
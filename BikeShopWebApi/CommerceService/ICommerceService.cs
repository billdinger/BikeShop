using System;
using BikeShopWebApi.CommerceService.Models;
using BikeShopWebApi.ProductService.Models;

namespace BikeShopWebApi.CommerceService
{
    public interface ICommerceService
    {
        void Purchase(Guid cartId);

        Cart Get(Guid cartId);

        void Add(Product product, Guid cartId);

        void Remove(Guid cartId);

    }
}
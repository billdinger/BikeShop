using System.Collections.Generic;
using BikeShopWebApi.ProductService.Models;

namespace BikeShopWebApi.ProductService
{
    public interface IProductService
    {
        IList<Product> GetAllProducts();

        IList<Product> Search(string query);
    }
}
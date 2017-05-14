using System;

namespace BikeShopWebApi.ProductService.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Uri Image { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

    }
}
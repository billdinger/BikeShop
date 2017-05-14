using System;
using System.Collections.Generic;
using BikeShopWebApi.ProductService.Models;

namespace BikeShopWebApi.CommerceService.Models
{
    public class Cart
    {

        public Guid CartId { get; set; }

        public IList<Product> Products { get; set; }
        
        public DateTime LastModified { get; set; }

        public bool Purchase { get; set; }
    }
}
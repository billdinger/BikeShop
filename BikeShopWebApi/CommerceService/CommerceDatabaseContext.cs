using System;
using System.Data.Entity;
using BikeShopWebApi.CommerceService.Models;

namespace BikeShopWebApi.CommerceService
{
    public class CommerceDatabaseContext : DbContext
    {

        public virtual DbSet<Cart> Carts { get; set; }
    }
}
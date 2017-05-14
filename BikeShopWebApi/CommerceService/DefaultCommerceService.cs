using System;
using System.Data.Entity;
using System.Linq;
using BikeShopWebApi.CommerceService.Models;
using BikeShopWebApi.ProductService.Models;

namespace BikeShopWebApi.CommerceService
{
    public class DefaultCommerceService : ICommerceService
    {

        public CommerceDatabaseContext DbContext { get; }

        public DefaultCommerceService(CommerceDatabaseContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            DbContext = dbContext;
        }



        public void Purchase(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(cartId));
            }

            // DB logic here.
        }

        public Cart Get(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(cartId));
            }

            return DbContext.Carts.SingleOrDefault(x => x.CartId == cartId);
        }

        public void Add(Product product, Guid cartId)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (cartId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(cartId));
            }

            Cart cart = Get(cartId);
            cart.Products.Add(product);
            DbContext.SaveChanges();
        }

        public void Remove(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(cartId));
            }
            Cart cart = Get(cartId);
            DbContext.Carts.Remove(cart);
            DbContext.SaveChanges();
        }
    }
}
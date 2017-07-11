using System;
using System.Web.Http;
using BikeShopWebApi.ProductService;
using Castle.Core.Logging;

namespace BikeShopWebApi.Controllers
{
    [Route("api/v1/products")]
    public class ProductsController : ApiController
    {
        private IProductService ProductService { get; }

        private ILogger Logger { get; }


        public ProductsController(IProductService productService, ILogger logger)
        {
            if (productService == null)
            {
                throw new ArgumentNullException(nameof(productService));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            ProductService = productService;
            Logger = logger;
        }


        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var products = ProductService.GetAllProducts();

                return Ok(products);

            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing {nameof(ProductsController)} action {nameof(Get)}", ex);
                return InternalServerError();
            }

        }

        [HttpGet]
        public IHttpActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest();
            }

            try
            {
                var products = ProductService.Search(query);

                return Ok(products);

            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing {nameof(ProductsController)} action {nameof(Search)}", ex);
                return InternalServerError();
            }
        }
    }
}
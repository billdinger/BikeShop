using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using BikeShopWebApi.Cache;
using BikeShopWebApi.CommerceService;
using BikeShopWebApi.ProductService;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BikeShopWebApi.DependencyInjection
{
    public class BikeShopWebApiInstaller : IWindsorInstaller
    {
        /// <summary>
        /// This is our Composition root for Castle Windsor, used to setup all our depdencies that
        /// our application uses. Add each dependency here and it will be added tou our container.
        /// </summary>
        /// <param name="container">The <see cref="T:Castle.Windsor.IWindsorContainer"/> that we add services too.</param>
        /// <param name="store">The <see cref="T:Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore"/></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            // Adds Log4net to be used whenever an INstance of ILogger is called.
            container.AddFacility<LoggingFacility>(x => x.UseLog4Net());

            // Setups every controller as a transient lifestyle object.
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .LifestyleTransient());

            // register our services.
            container.Register(
                Component.For<ICache>()
                    .ImplementedBy(typeof(CacheShim))
                    .LifestyleSingleton(),
                Component.For<IProductService>()
                    .ImplementedBy(typeof(DefaultProductService))
                    .LifestyleSingleton(),
                Component.For<ICommerceService>()
                    .ImplementedBy(typeof(DefaultCommerceService))
                    .LifestyleSingleton());

            container.Register(
                Component.For<CommerceDatabaseContext>()
                    .ImplementedBy(typeof(CommerceDatabaseContext))
                    .LifestyleSingleton());

            // register our handler, used by the HTTP Client.
            container.Register(
                Component.For<HttpMessageHandler>()
                .LifestyleSingleton()
                .UsingFactoryMethod(
                    () => new HttpClientHandler()));

            // register our HTTPContext
            container.Register(
                Component.For<HttpContextBase>()
                    .LifestylePerWebRequest()
                    .UsingFactoryMethod(
                        () => new HttpContextWrapper(HttpContext.Current)));

        }
    }
}
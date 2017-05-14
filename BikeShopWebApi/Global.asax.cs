using System.Web.Http;
using System.Web.Http.Dispatcher;
using BikeShopWebApi.DependencyInjection;
using Castle.Windsor;

namespace BikeShopWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Perform our container installation.
            var container = new WindsorContainer();
            container.Install(new BikeShopWebApiInstaller());

            // Set our HttpControllerActivator to our created on that uses dependency injection.
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorControllerActivator(container));
        }
    }
}

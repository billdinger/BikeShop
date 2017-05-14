using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;

namespace BikeShopWebApi.DependencyInjection
{
    public class WindsorControllerActivator : IHttpControllerActivator
    {

        private IWindsorContainer Container { get; }

        /// <summary>
        /// Creates a <see cref="T:BikeShopWebApi.DependencyInjection.WindsorControllerActivator"/> that is used to create & dispose of controllers from our Castle Windsor
        /// DI Container.
        /// </summary>
        /// <param name="container">A <see cref="T:Castle.Windsor.IWindsorContainer"/> that is used to Resolve and Release the requests. Cannot be null.</param>
        public WindsorControllerActivator(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            Container = container;
        }



        /// <summary>
        /// Used by the ASP.NET Framework to create an IHTTP Controller. We override it to allow our Castle Windsor to instantiate the controller
        /// and any of its depedencies needed to process the request. Once the controller is finished with the request we release it from our
        /// container to prevent a memory leak.
        /// </summary>
        /// <param name="request">A <see cref="T: System.Net.Http.HttpRequestMessage"/> cannot be null.</param>
        /// <param name="controllerDescriptor">A <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/> cannot be null</param>
        /// <param name="controllerType">A <see cref="T:System.Type"/> of the controller. Cannot be null.</param>
        /// <returns>A <see cref="T:System.Web.Http.Controllers.IHttpController"/> that matches the requested type.</returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (controllerDescriptor == null)
            {
                throw new ArgumentNullException(nameof(controllerDescriptor));
            }
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            var controller =
                (IHttpController)Container.Resolve(controllerType);

            request.RegisterForDispose(new WindsorRelease(() => Container.Release(controller)));

            return controller;
        }
    }
}
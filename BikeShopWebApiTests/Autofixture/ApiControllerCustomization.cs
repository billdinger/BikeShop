using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using AutoFixture;

namespace BikeShopWebApiTests.Autofixture
{
    /// <summary>
    /// Web API Controllers contain a LOT of properties, this is needed to prevent autofixture from
    /// attempting to automoq and create all your depdencies for you.
    /// </summary>
    public class ApiControllerCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<HttpConfiguration>(
                c => c
                    .OmitAutoProperties());
            fixture.Customize<HttpRequestMessage>(
                c => c
                    .Do(
                        x =>
                            x.Properties.Add(
                                HttpPropertyKeys.HttpConfigurationKey,
                                fixture.Create<HttpConfiguration>())));
            fixture.Customize<HttpRequestContext>(
                c => c
                    .Without(x => x.ClientCertificate));
        }

    }
}
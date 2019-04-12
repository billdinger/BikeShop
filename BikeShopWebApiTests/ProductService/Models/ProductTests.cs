using BikeShopWebApi.ProductService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;

namespace BikeShopWebApiTests.ProductService.Models
{
    [TestClass]
    [TestCategory("Products")]
    public class ProductTests
    {
        private IFixture Fixture { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization(){ConfigureMembers = true});
        }

        [TestMethod]
        public void Properties_VerifyAssigned()
        {
            // arrange
            var assertion = new WritablePropertyAssertion(Fixture);

            // act
            var sut = Fixture.Create<Product>();
            var props = sut.GetType().GetProperties();

            // assert.
            assertion.Verify(props);
        }


    }
}

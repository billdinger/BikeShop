using BikeShopWebApi.CommerceService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.CommerceService.Models
{
    [TestClass]
    [TestCategory("CommerceService")]
    public class CartTests
    {

        private IFixture Fixture { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public void Properties_VerifyAssigned()
        {
            // arrange
            var assertion = new WritablePropertyAssertion(Fixture);

            // act
            var sut = Fixture.Create<Cart>();
            var props = sut.GetType().GetProperties();

            // assert.
            assertion.Verify(props);
        }
    }
}
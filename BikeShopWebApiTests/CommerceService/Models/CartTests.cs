using System;
using BikeShopWebApi.CommerceService.Models;
using BikeShopWebApi.ProductService.Models;
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

        /// <summary>
        /// Used to demo the capabilites of autofixtures
        /// Customize Mechanism.
        /// </summary>
        [TestMethod]
        public void Cart_ModifyBuild()
        {
            // arrange
            Fixture.Customize<Cart>(ob => ob
                .With(x => x.Purchase, false)
                .With(gu => gu.CartId, Fixture.Freeze<Guid>())
                .With(t => t.LastModified, DateTime.MaxValue));

            // act
            var sut1 = Fixture.Create<Cart>();
            var sut2 = Fixture.Create<Cart>();
            
            // assert.
            Assert.AreEqual(sut1.Purchase, sut2.Purchase);
            Assert.AreEqual(sut1.CartId, sut2.CartId);
            Assert.AreEqual(sut1.LastModified, sut2.LastModified);
        }
    }
}
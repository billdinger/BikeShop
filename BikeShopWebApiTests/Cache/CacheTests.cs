using BikeShopWebApi.ProductService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.Cache
{
    [TestClass]
    [TestCategory("Cache")]
    public class CacheTests
    {
        private IFixture Fixture { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public void Set_Object_Void()
        {
            // act
            BikeShopWebApi.Cache.Cache.Set(Fixture.Create<Product>(), Fixture.Create<string>(), Fixture.Create<int>());

        }

        [TestMethod]
        public void Set_GaurdClause_Throws()
        {
 
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var method = typeof(BikeShopWebApi.Cache.Cache).GetMethod(nameof(BikeShopWebApi.Cache.Cache.Set));

            // assert
            assertion.Verify(method);

        }

        [TestMethod]
        public void Get_Key_Object()
        {
            // arrange
            var cacheItem = Fixture.Create<object>();
            var key = Fixture.Create<string>();
            BikeShopWebApi.Cache.Cache.Set(cacheItem, key, Fixture.Create<int>());

            // act
            var result =  BikeShopWebApi.Cache.Cache.Get<object>(key);
            
            // assert.
            Assert.AreEqual(cacheItem,result );

        }

        [TestMethod]
        public void Get_GaurdClause_Throws()
        {

            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var method = typeof(BikeShopWebApi.Cache.Cache).GetMethod(nameof(BikeShopWebApi.Cache.Cache.Get));

            // assert
            assertion.Verify(method);

        }

    }
}
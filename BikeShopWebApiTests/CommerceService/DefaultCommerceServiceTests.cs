using BikeShopWebApi.CommerceService;
using BikeShopWebApiTests.Autofixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.CommerceService
{
    [TestClass]
    [TestCategory("CommerceService")]
    public class DefaultCommerceServiceTests
    {
        private IFixture Fixture { get; set; }

   
        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());
        }


        [TestMethod]
        public void Constructor_GuardClause_Throws()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);
            Fixture.ResidueCollectors.Add(
                new MockRelay(
                    new DbSetSpecification()));
            // act
            var ctors = typeof(DefaultCommerceService).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }
    }
}
using BikeShopWebApi.DependencyInjection;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.DependencyInjection
{
    [TestClass]
    [TestCategory("DepedendencyInjection")]
    public class WindsorControllerActivatorTests
    {
        private IFixture Fixture { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
        }

        [TestMethod]
        public void Properties_VerifyAssigned()
        {
            // arrange
            var assertion = new WritablePropertyAssertion(Fixture);
            Fixture.Inject<IWindsorContainer>(new WindsorContainer());
            var sut = Fixture.Create<WindsorControllerActivator>();

            // act
            var props = sut.GetType().GetProperties();

            // assert.
            assertion.Verify(props);
        }

        [TestMethod]
        public void Constructor_GuardClause_Throws()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);
            Fixture.Inject<IWindsorContainer>(new WindsorContainer());


            // act
            var ctors = typeof(WindsorControllerActivator).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }

    }
}
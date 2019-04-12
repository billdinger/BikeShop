using System.Linq;
using BikeShopWebApi.DependencyInjection;
using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;

namespace BikeShopWebApiTests.DependencyInjection
{
    [TestClass]
    [TestCategory("DepedendencyInjection")]
    public class BikeShopWebApiInstallerTests
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
            var sut = Fixture.Create<BikeShopWebApiInstaller>();
            var props = sut.GetType().GetProperties();

            // assert.
            assertion.Verify(props);
        }

        [TestMethod]
        public void Constructor_GuardClause_Throws()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var ctors = typeof(BikeShopWebApiInstaller).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }

        [TestMethod]
        public void Install_GuardClause_Throws()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);
            Fixture.Inject<IWindsorContainer>(new WindsorContainer());

            // act
            var ctors = typeof(BikeShopWebApiInstaller).GetMethod(nameof(BikeShopWebApiInstaller.Install));

            // assert
            assertion.Verify(ctors);
        }

        [TestMethod]
        public void Initialize_Void_NoMisconfiguredComponents()
        {
            // arrange
            var container = new WindsorContainer();
            container.Install(new BikeShopWebApiInstaller());

            // act.
            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var diagnostics = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>();
            var handlers = diagnostics.Inspect();

            // assert.
            Assert.AreEqual(false, handlers.Any());
        }

        [TestMethod]
        public void Initialize_Void_NoLifeCycleMismatches()
        {
            // arrange
            var container = new WindsorContainer();
            container.Install(new BikeShopWebApiInstaller());


            // act.
            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var diagnostics = host.GetDiagnostic<IPotentialLifestyleMismatchesDiagnostic>();
            var handlers = diagnostics.Inspect();

            // assert.
            Assert.AreEqual(false, handlers.Any());
        }

    }
}
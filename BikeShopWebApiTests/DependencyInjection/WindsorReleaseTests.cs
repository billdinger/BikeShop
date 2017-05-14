using System;
using BikeShopWebApi.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.DependencyInjection
{
    [TestClass]
    [TestCategory("DepedendencyInjection")]
    public class WindsorReleaseTests
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
            var sut = Fixture.Create<WindsorRelease>();
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
            var ctors = typeof(WindsorRelease).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }

        [TestMethod]
        public void Dispose_Void_ActionCalled()
        {
            // arrange
            var mockAction = new Mock<Action>();
            var mockDelegate = new Mock<Func<Action>>();
            mockDelegate.Setup(x => x()).Returns(mockAction.Object);
            var sut = new WindsorRelease(mockDelegate.Object());

            // act.
            sut.Dispose();
            
            // assert.
            mockAction.Verify(x => x());

        }

        [TestMethod]
        public void Dispose_Void_ActionCalledOnce()
        {
            // arrange
            var mockAction = new Mock<Action>();
            var mockDelegate = new Mock<Func<Action>>();
            mockDelegate.Setup(x => x()).Returns(mockAction.Object);
            var sut = new WindsorRelease(mockDelegate.Object());

            // act.
            sut.Dispose();
            sut.Dispose();

            // assert.
            mockAction.Verify(x => x(), Times.Once);
        }

    }
}
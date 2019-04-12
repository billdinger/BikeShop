using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace BikeShopWebApiTests.Adder
{
    [TestClass]
    [TestCategory("Adder")]
    public class AdderTests
    {
        private IFixture Fixture { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void Add_OneOne_Two()
        {
            // arrange
            int a = 1;
            int b = 1;
            var sut = new BikeShopWebApi.Adder.Adder();

            // act
            long result = sut.Add(a, b);

            // assert.
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Multiply_ThreeSeven_21()
        {
            // arrange
            int a = 3;
            int b = 7;
            var sut = new BikeShopWebApi.Adder.Adder();

            // act
            long result = sut.Multiply(a, b);

            // assert.
            Assert.AreEqual(21, result);
        }

        [TestMethod]
        public void Add_Int_Long()
        {
            // arrange
            var sut = Fixture.Create<BikeShopWebApi.Adder.Adder>();

            // act
            long result =
                sut.Add(Fixture.Create<int>(), Fixture.Create<int>());

            // assert.
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void Multiply_Int_Log()
        {
            // arrange.
            Fixture.Inject<int>(8);
            var sut = Fixture.Create<BikeShopWebApi.Adder.Adder>();

            // act
            long result =
                sut.Multiply(Fixture.Create<int>(), Fixture.Create<int>());

            // assert.
            Assert.AreNotEqual(0, result);
            // act

        }
    }
}
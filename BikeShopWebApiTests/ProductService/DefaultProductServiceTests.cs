using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BikeShopWebApi.Cache;
using BikeShopWebApi.ProductService;
using BikeShopWebApi.ProductService.Exceptions;
using BikeShopWebApi.ProductService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.ProductService
{
    [TestClass]
    [TestCategory("Products")]
    public class DefaultProductServiceTests
    {
        private IFixture Fixture { get; set; }

        private const string SerializedFakeProducts =
                "[{\"Id\":109,\"Description\":\"Description1bd5b031-0524-4f7c-b172-fddc0fe05111\",\"Price\":18.0,\"Image\":\"http://ea57fd91-c82e-4406-899e-8b51278e904b\",\"Name\":\"Name795e6a66-69b5-4822-ba70-07e0d1e340d5\",\"Quantity\":11},{\"Id\":216,\"Description\":\"Descriptionfe61f009-2b0c-4bf2-b195-5bf9410806a3\",\"Price\":12.0,\"Image\":\"http://170bc0f1-c841-41dd-b47e-030d22778698\",\"Name\":\"Name9100a786-5124-45b9-a9d7-3dc3f4418b91\",\"Quantity\":74},{\"Id\":221,\"Description\":\"Description69a79664-26d9-4d69-ae8f-7c215b8a3b2a\",\"Price\":179.0,\"Image\":\"http://24297c0a-81fc-4166-bd45-6921ae13605f\",\"Name\":\"Name3e7db1f5-003f-4b24-bd55-827dbd4d748c\",\"Quantity\":251}]";


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

            // act
            var ctors = typeof(DefaultProductService).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }

        [TestMethod]
        public void GetAllProducts_Void_Products()
        {
            // arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(SerializedFakeProducts) }));

            Fixture.Inject<HttpMessageHandler>(handler.Object);
            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => null);

            var sut = Fixture.Create<DefaultProductService>();

            // act
            var result = sut.GetAllProducts();

            // assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            var cache = Fixture.Create<Mock<ICache>>();
            cache.Verify(x => x.Set(It.IsAny<IList<Product>>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void GetAllProducts_VoidInCache_Products()
        {
            //arrange
            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => Fixture.Create<IList<Product>>());

            // act
            var sut = Fixture.Create<DefaultProductService>();
            var result = sut.GetAllProducts();

            // assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NoProductsFoundException))]
        public void GetAllProducts_NullContent_NoProductsFoundException()
        {
            // arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));


            Fixture.Inject<HttpMessageHandler>(handler.Object);
            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => null);

            var sut = Fixture.Create<DefaultProductService>();

            // act
            var result = sut.GetAllProducts();

            // assert.
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductErrorException))]
        public void GetAllProducts_HttpStatusCodeNotOk_ProductErrorException()
        {
            // arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));


            Fixture.Inject<HttpMessageHandler>(handler.Object);
            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => null);

            var sut = Fixture.Create<DefaultProductService>();

            // act
            var result = sut.GetAllProducts();

            // assert.
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Search_Void_Products()
        {
            // arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(SerializedFakeProducts) }));

            Fixture.Inject<HttpMessageHandler>(handler.Object);
            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => null);

            var sut = Fixture.Create<DefaultProductService>();

            // act
            var result = sut.Search(Fixture.Freeze<string>());

            // assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            var cache = Fixture.Create<Mock<ICache>>();
            cache.Verify(x => x.Set(It.IsAny<IList<Product>>(), Fixture.Create<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void Search_VoidInCache_Products()
        {
            //arrange

            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => Fixture.Create<IList<Product>>());

            var sut = Fixture.Create<DefaultProductService>();

            // act
            var result = sut.Search(Fixture.Freeze<string>());

            // assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchErrorException))]
        public void Search_HttpStatusCodeNotOk_ProductErrorException()
        {
            // arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));


            Fixture.Inject<HttpMessageHandler>(handler.Object);
            Fixture.Freeze<Mock<ICache>>()
                .Setup(x => x.Get<IList<Product>>(It.IsAny<string>()))
                .Returns(() => null);

            var sut = Fixture.Create<DefaultProductService>();

            // act
            var result = sut.Search(Fixture.Freeze<string>());

            // assert.
            Assert.IsNull(result);
        }

    }


}
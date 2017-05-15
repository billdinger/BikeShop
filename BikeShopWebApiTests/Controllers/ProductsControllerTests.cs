using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using BikeShopWebApi.Controllers;
using BikeShopWebApi.ProductService;
using BikeShopWebApi.ProductService.Models;
using BikeShopWebApiTests.Autofixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;

namespace BikeShopWebApiTests.Controllers
{
    [TestClass]
    [TestCategory("Controllers")]
    public class ProductsControllerTests
    {

        private IFixture Fixture { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Fixture = new Fixture();
            Fixture.Customize(new ApiControllerCustomization());
            Fixture.Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void Properties_VerifyAssigned()
        {
            // arrange
            var assertion = new WritablePropertyAssertion(Fixture);

            // act
            var sut = Fixture.Create<ProductsController>();
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
            var ctors = typeof(ProductsController).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }

        [TestMethod]
        public void Get_Void_OK()
        {
            // arrange
            Fixture.Freeze<Mock<IProductService>>().Setup(x => x.GetAllProducts())
                .Returns(Fixture.Freeze<IList<Product>>());
            var sut = Fixture.Create<ProductsController>();

            // act
            var result = sut.Get().ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var service = Fixture.Create<Mock<IProductService>>();
            service.Verify(x => x.GetAllProducts(), Times.Once);
        }

        [TestMethod]
        public void Get_Throws_InternalServerError()
        {
            // arrange
            Fixture.Freeze<Mock<IProductService>>().Setup(x => x.GetAllProducts())
                .Throws(new Exception(Fixture.Create<string>()));
            var sut = Fixture.Create<ProductsController>();

            // act
            var result = sut.Get().ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<IProductService>>();
            service.Verify(x => x.GetAllProducts(), Times.Once);
        }

        [TestMethod]
        public void Search_EmptyString_BadRequest()
        {
            // arrange
            Fixture.Freeze<Mock<IProductService>>();
            var sut = Fixture.Create<ProductsController>();

            // act
            var result = sut.Search(string.Empty).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var service = Fixture.Create<Mock<IProductService>>();
            service.Verify(x => x.GetAllProducts(), Times.Never);
        }

        [TestMethod]
        public void Search_Throws_InternalServerError()
        {
            // arrange
            Fixture.Freeze<Mock<IProductService>>().Setup(x => x.Search(It.IsAny<string>()))
                .Throws(new Exception(Fixture.Create<string>()));
            var sut = Fixture.Create<ProductsController>();

            // act
            var result = sut.Search(Fixture.Create<string>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<IProductService>>();
            service.Verify(x => x.Search(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Search_String_Ok()
        {
            // arrange
            Fixture.Freeze<Mock<IProductService>>().Setup(x => x.Search(Fixture.Freeze<string>()))
                .Returns(Fixture.Freeze<IList<Product>>());
            var sut = Fixture.Create<ProductsController>();

            // act
            var result = sut.Search(Fixture.Create<string>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var service = Fixture.Create<Mock<IProductService>>();
            service.Verify(x => x.Search(Fixture.Create<string>()), Times.Once);
        }

    }
}
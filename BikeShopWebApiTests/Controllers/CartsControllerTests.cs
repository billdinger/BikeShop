using System;
using System.Net;
using System.Threading;
using BikeShopWebApi.CommerceService;
using BikeShopWebApi.CommerceService.Models;
using BikeShopWebApi.Controllers;
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
    public class CartsControllerTests
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
            var sut = Fixture.Create<CartsController>();
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
            var ctors = typeof(CartsController).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }


        [TestMethod]
        public void Get_Guid_OK()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns(Fixture.Freeze<Cart>());
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Get(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Get(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Get_Throws_ServerError()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Throws(new Exception(Fixture.Create<string>()));
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Get(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Get(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Get_EmptyGuid_BadRequest()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>();
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Get(Guid.Empty).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Get(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Add_GuidProduct_Ok()
        {
            // arrange
            Fixture.Customize<Product>(custom => custom.With(product => product.Id, 3));
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Add(It.Is<Product>(z => z.Id.Equals(3)), It.IsAny<Guid>()));
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Add(Fixture.Create<Product>(), Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Add(It.Is<Product>(z => z.Id.Equals(3)), It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Add_Exception_InternalServerError()
        {
            // arrange
            Fixture.Customize<Product>(custom => custom.With(product => product.Id, 3));
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Add(It.Is<Product>(z => z.Id.Equals(3)), It.IsAny<Guid>()))
                .Throws(new Exception(Fixture.Create<string>()));
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Add(Fixture.Create<Product>(), Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Add(It.Is<Product>(z => z.Id.Equals(3)), It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Add_NullProduct_BadRequest()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>();
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Add(null, Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Add(It.IsAny<Product>(), It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Add_EmptyGuid_BadRequest()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>();
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Add(Fixture.Create<Product>(), Guid.Empty).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Add(It.IsAny<Product>(), It.IsAny<Guid>()), Times.Never);
        }


        [TestMethod]
        public void Delete_Guid_Ok()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Remove(It.IsAny<Guid>()));
            var sut = Fixture.Create<CartsController>();

            // act
            var result = sut.Delete(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Delete_Exception_InternalServerError()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Remove(It.IsAny<Guid>()))
                .Throws(new Exception(Fixture.Create<string>()));
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Delete(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Delete_EmptyGuid_BadRequest()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>();
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Delete(Guid.Empty).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Post_Guid_Ok()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Purchase(It.IsAny<Guid>()));
            var sut = Fixture.Create<CartsController>();

            // act
            var result = sut.Post(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Purchase(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Post_Exception_InternalServerError()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>()
                .Setup(x => x.Purchase(It.IsAny<Guid>()))
                .Throws(new Exception(Fixture.Create<string>()));
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Post(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Purchase( It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void Post_EmptyGuid_BadRequest()
        {
            // arrange
            Fixture.Freeze<Mock<ICommerceService>>();
            var sut = Fixture.Create<CartsController>();


            // act
            var result = sut.Post(Guid.Empty).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Purchase(It.IsAny<Guid>()), Times.Never);
        }

    }
}
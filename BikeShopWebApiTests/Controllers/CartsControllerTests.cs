using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using BikeShopWebApi.CommerceService;
using BikeShopWebApi.CommerceService.Models;
using BikeShopWebApi.Controllers;
using BikeShopWebApi.ProductService.Models;
using BikeShopWebApiTests.Autofixture;
using Castle.Services.Logging.Log4netIntegration;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;
using ILogger = Castle.Core.Logging.ILogger;

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

        // used to show what a test looks like without
        // any mocking framework or mock container.
        [TestMethod]
        public void Constructor_AllServicesManually_CartsController()
        {
            // arrange
            var httpRequest = new HttpRequest("", "https://www.vml.com", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            HttpContextBase contextBase = new HttpContextWrapper(httpContext);
            ICommerceService service = new DefaultCommerceService(new CommerceDatabaseContext());
            ILogger logger = new Log4netLogger(new RootLogger(Level.Alert),new Log4netFactory() );

            // act
            var sut = new CartsController(service, logger, contextBase);

            // assert
            Assert.IsNotNull(sut);
        }

        /// <summary>
        /// Used to demonstrate what a test looks like without
        /// Autofixture
        /// </summary>
        [TestMethod]
        public void Constructor_AllServicesMock_CartsController()
        {
            // arrange
            var mockLogger = new Mock<ILogger>();
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockCommerceService = new Mock<ICommerceService>();

            // act
            var sut =
                new CartsController(mockCommerceService.Object, mockLogger.Object,
                mockHttpContext.Object);

            // assert
            Assert.IsNotNull(sut);
        }

        

        /// <summary>
        /// Used to demonstrate difference between autofixture and manually mocking.
        /// </summary>
        [TestMethod]
        public void Constructor_AllServices_CartsController()
        {
            // act
            var sut = Fixture.Create<CartsController>();

            // assert
            Assert.IsNotNull(sut);
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
            var result = sut.Get(Fixture.Create<Guid>())
                .ExecuteAsync(new CancellationToken()).Result;

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
                .Setup(
                x => x.Add(
                    It.Is<Product>(z => z.Id.Equals(3)), It.IsAny<Guid>()));
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
                .Setup(x =>
                x.Add(It.Is<Product>(z => z.Id.Equals(3)), It.IsAny<Guid>()))
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

            // setup our fake HTTP context
            Fixture.Freeze<Mock<HttpSessionStateBase>>()
                .Setup(x => x.SessionID)
                .Returns(Fixture.Freeze<string>());
            Fixture.Freeze<Mock<HttpContextBase>>()
                .Setup(x => x.Session)
                .Returns(Fixture.Create<HttpSessionStateBase>());

            // act
            var sut = Fixture.Create<CartsController>();
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

            // setup our fake HTTP context
            Fixture.Freeze<Mock<HttpSessionStateBase>>()
                .Setup(x => x.SessionID)
                .Returns(Fixture.Freeze<string>());
            Fixture.Freeze<Mock<HttpContextBase>>()
                .Setup(x => x.Session)
                .Returns(Fixture.Create<HttpSessionStateBase>());

            // act
            var sut = Fixture.Create<CartsController>();
            var result = sut.Post(Fixture.Create<Guid>()).ExecuteAsync(new CancellationToken()).Result;

            // assert.
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            var service = Fixture.Create<Mock<ICommerceService>>();
            service.Verify(x => x.Purchase(It.IsAny<Guid>()), Times.Once);
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
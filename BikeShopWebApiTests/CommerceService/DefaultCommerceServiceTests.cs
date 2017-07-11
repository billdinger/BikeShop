using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BikeShopWebApi.CommerceService;
using BikeShopWebApi.CommerceService.Models;
using BikeShopWebApi.ProductService.Models;
using BikeShopWebApiTests.Autofixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            Fixture.ResidueCollectors.Add(
                new MockRelay(
                    new DbSetSpecification()));
        }


        [TestMethod]
        public void Constructor_GuardClause_Throws()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var ctors = typeof(DefaultCommerceService).GetConstructors();

            // assert
            assertion.Verify(ctors);
        }

        [TestMethod]
        public void Get_Guid_Cart()
        {
            // arrange
            Fixture.RepeatCount = 1;
            Fixture.Build<Cart>().With(z => z.CartId, Fixture.Freeze<Guid>());
            var dbset = CreateDbSetMock(Fixture.Create<IList<Cart>>());
            Fixture.Inject<CommerceDatabaseContext>(
                new CommerceDatabaseContext() { Carts = dbset.Object });

            // act
            var sut = Fixture.Create<DefaultCommerceService>();
            var result = sut.Get(Fixture.Create<Guid>());

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Get_EmptyGuid_GuardClause()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var method = typeof(DefaultCommerceService).GetMethod(nameof(DefaultCommerceService.Get));

            // assert
            assertion.Verify(method);
        }

        [TestMethod]
        public void Add_GuidProduct_Void()
        {
            // arrange
            Fixture.RepeatCount = 1;
            Fixture.Build<Cart>().With(z => z.CartId, Fixture.Freeze<Guid>());
            var dbset = CreateDbSetMock(Fixture.Create<IList<Cart>>());
            Fixture.Inject<CommerceDatabaseContext>(new CommerceDatabaseContext() { Carts = dbset.Object });

            // act
            var sut = Fixture.Create<DefaultCommerceService>();
            sut.Add(Fixture.Create<Product>(), Fixture.Create<Guid>());

            // assert
            var context = Fixture.Create<CommerceDatabaseContext>();
            Assert.AreEqual(2, context.Carts.SingleOrDefault().Products.Count);

        }

        [TestMethod]
        public void Add_EmptyGuidNullProduct_GuardClause()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var method = typeof(DefaultCommerceService).GetMethod(nameof(DefaultCommerceService.Add));

            // assert
            assertion.Verify(method);
        }

        [TestMethod]
        public void Purchase_Guid_Void()
        {
            // arrange
            Fixture.RepeatCount = 1;
            Fixture.Build<Cart>().With(z => z.CartId, Fixture.Freeze<Guid>());
            var dbset = CreateDbSetMock(Fixture.Create<IList<Cart>>());
            Fixture.Inject<CommerceDatabaseContext>(new CommerceDatabaseContext() { Carts = dbset.Object });

            // act
            var sut = Fixture.Create<DefaultCommerceService>();
            sut.Purchase(Fixture.Create<Guid>());

            // assert
            var context = Fixture.Create<CommerceDatabaseContext>();
            Assert.AreEqual(true, context.Carts.SingleOrDefault().Purchase);

        }


        [TestMethod]
        public void Purchase_EmptyGuid_GuardClause()
        {
            // arrange
            var assertion = new GuardClauseAssertion(Fixture);

            // act
            var method = typeof(DefaultCommerceService).GetMethod(nameof(DefaultCommerceService.Purchase));

            // assert
            assertion.Verify(method);
        }


        /// <summary>
        /// See here: http://www.jankowskimichal.pl/en/2016/01/mocking-dbcontext-and-dbset-with-moq/ for more details.
        /// </summary>
        private static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }
    }
}
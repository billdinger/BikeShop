using System;
using System.Data.Entity;
using AutoFixture.Kernel;

namespace BikeShopWebApiTests.Autofixture
{
    public class DbSetSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
                return false;

            return type.IsGenericType
                   && typeof(DbSet<>) == type.GetGenericTypeDefinition();
        }
    }
}
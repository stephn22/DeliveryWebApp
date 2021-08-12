using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace DeliveryWebApp.Domain.UnitTests.Product
{
    public class ProductTests
    {

        [Fact]
        public void Discount()
        {
            var p1 = new Entities.Product {Price = 22.50M, Discount = 10};
            var p2 = new Entities.Product {Price = 25.50M, Discount = 0};

            //var finalPrice = p1.ApplyDiscount(); TODO: complete tests
            //var noDiscount = p2.ApplyDiscount();

            //Assert.Equal(20.25M, finalPrice);
            //Assert.Equal(25.50M, noDiscount);
        }

        [Fact]
        public void TotalPriceWithDiscount()
        {
            var list = new List<Entities.Product>
            {
                new() {Price = 25.30M, Quantity = 37, Discount = 30},
                new() {Price = 11.75M, Quantity = 21, Discount = 20},
                new() {Price = 21.30M, Quantity = 11, Discount = 15},
                new() {Price = 35.25M, Quantity = 16, Discount = 0}
            };

            //var tot = Entities.Product.TotalPrice(list); TODO: complete tests

            //Assert.Equal(1615.82M, tot);
        }
    }
}

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
            var p1 = new Entities.Product {Price = 22.50, Discount = 10};
            var p2 = new Entities.Product {Price = 25.50, Discount = 0};

            var finalPrice = p1.ApplyDiscount();
            var noDiscount = p2.ApplyDiscount();

            Assert.Equal(20.25, finalPrice);
            Assert.Equal(25.50, noDiscount);
        }

        [Fact]
        public void TotalPriceWithDiscount()
        {
            var list = new List<Entities.Product>
            {
                new() {Price = 25.30, Quantity = 37, Discount = 30},
                new() {Price = 11.75, Quantity = 21, Discount = 20},
                new() {Price = 21.30, Quantity = 11, Discount = 15},
                new() {Price = 35.25, Quantity = 16, Discount = 0}
            };

            var tot = Entities.Product.TotalPrice(list);

            Assert.Equal(1615.82, tot);
        }
    }
}

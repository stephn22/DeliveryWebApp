using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string StateProvince { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(AddressLine2)
                ? $"{AddressLine1}, {Number}, {City}, {PostalCode}, {StateProvince}, {Country}"
                : $"{AddressLine1}, {AddressLine2}, {Number}, {City}, {PostalCode}, {StateProvince}, {Country}";
        }
    }
}
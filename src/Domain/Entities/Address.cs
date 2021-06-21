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

        public Address(string addressLine1, string addressLine2, string number, string city, string postalCode, string country)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Number = number;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }

        public override string ToString()
        {
            return $"{AddressLine1}, {AddressLine2}, {Number}, {City}, {PostalCode}, {Country}";
        }
    }
}

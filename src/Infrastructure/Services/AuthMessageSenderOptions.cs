using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;

namespace DeliveryWebApp.Infrastructure.Services
{
    public class AuthMessageSenderOptions : IAuthMessageSenderOptions
    {
        public string User { get; set; }
        public string Key { get; set; }
    }
}

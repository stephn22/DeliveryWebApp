using DeliveryWebApp.Application.Common.Interfaces;
using System;

namespace DeliveryWebApp.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
using System;

namespace DeliveryWebApp.Domain.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}
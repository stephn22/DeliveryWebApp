using DeliveryWebApp.Domain.Common;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
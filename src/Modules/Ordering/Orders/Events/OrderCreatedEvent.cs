using MassTransit.Transports;
using Shared.DDD;

namespace Ordering.Orders.Events;
public record OrderCreatedEvent(Order Order)
    : IDomainEvent;
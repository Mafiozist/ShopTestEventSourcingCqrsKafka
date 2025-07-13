namespace ShopTestEventSourcingCqrsKafka.Common;

public interface IEventStore
{
    Task SaveEventAsync(Guid transactionId, object @event);
}

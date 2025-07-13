using ShopTestEventSourcingCqrsKafka.Common;

namespace ShopTestEventSourcingCqrsKafka.ShopService.Kafka;

public class KafkaTransactionListener : BackgroundService
{
    private readonly IKafkaConsumer _consumer;
    private readonly IEventStore _eventStore;

    public KafkaTransactionListener(IKafkaConsumer consumer, IEventStore eventStore)
    {
        _consumer = consumer;
        _eventStore = eventStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.SubscribeAsync("stock-reserved", async message =>
        {
            var transactionId = message.TransactionId;
            await _eventStore.SaveEventAsync(transactionId, new { Event = "StockReserved", message.ItemId });
        });
    }
}

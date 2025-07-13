using ShopTestEventSourcingCqrsKafka.Common;

public class TransactionListener : BackgroundService
{
    private readonly IKafkaConsumer _consumer;
    private readonly IKafkaProducer _producer;

    public TransactionListener(IKafkaConsumer consumer, IKafkaProducer producer)
    {
        _consumer = consumer;
        _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.SubscribeAsync("transactions", async message =>
        {
            var itemAvailable = true;
            if (itemAvailable)
            {
                await _producer.PublishAsync("stock-reserved", new
                {
                    message.TransactionId,
                    message.ItemId
                });
            }
        });
    }
}

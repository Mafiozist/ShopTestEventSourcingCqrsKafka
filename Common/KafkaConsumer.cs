using Confluent.Kafka;
using ShopTestEventSourcingCqrsKafka.Common;
using System.Text.Json;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly ConsumerConfig _config;

    public KafkaConsumer()
    {
        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "shop-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };
    }

    public async Task SubscribeAsync(string topic, Func<dynamic, Task> handler)
    {
        await Task.Run(() =>
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe(topic);

            while (true)
            {
                try
                {
                    var cr = consumer.Consume();
                    if (cr != null && !string.IsNullOrEmpty(cr.Message.Value))
                    {
                        var message = JsonSerializer.Deserialize<dynamic>(cr.Message.Value);
                        handler(message).Wait();
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Kafka consume error: {e.Error.Reason}");
                }
            }
        });
    }
}

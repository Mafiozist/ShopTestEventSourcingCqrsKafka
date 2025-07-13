using Confluent.Kafka;
using System.Text.Json;

namespace ShopTestEventSourcingCqrsKafka.Common;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer()
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync(string topic, object message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
    }
}

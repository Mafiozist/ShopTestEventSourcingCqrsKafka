namespace ShopTestEventSourcingCqrsKafka.Common;

public interface IKafkaProducer
{
    Task PublishAsync(string topic, object message);
}

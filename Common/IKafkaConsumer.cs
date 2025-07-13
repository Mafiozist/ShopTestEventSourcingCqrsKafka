namespace ShopTestEventSourcingCqrsKafka.Common;

public interface IKafkaConsumer
{
    Task SubscribeAsync(string topic, Func<dynamic, Task> handler);
}
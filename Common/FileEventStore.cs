using System.Text.Json;

namespace ShopTestEventSourcingCqrsKafka.Common;
public class FileEventStore : IEventStore
{
    public async Task SaveEventAsync(Guid transactionId, object @event)
    {
        var path = $"events/{transactionId}.json";
        var events = File.Exists(path)
            ? JsonSerializer.Deserialize<List<object>>(await File.ReadAllTextAsync(path))!
            : new List<object>();

        events.Add(@event);
        Directory.CreateDirectory("events");
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(events));
    }
}

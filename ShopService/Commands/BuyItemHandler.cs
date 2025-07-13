using MediatR;
using ShopTestEventSourcingCqrsKafka.Common;

namespace ShopTestEventSourcingCqrsKafka.ShopService.Commands;

public class BuyItemHandler : IRequestHandler<BuyItemCommand, Unit>
{
    private readonly IKafkaProducer _kafka;
    private readonly IEventStore _eventStore;

    public BuyItemHandler(IKafkaProducer kafka, IEventStore eventStore)
    {
        _kafka = kafka;
        _eventStore = eventStore;
    }

    public async Task<Unit> Handle(BuyItemCommand request, CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();

        await _eventStore.SaveEventAsync(transactionId, new
        {
            Event = "TransactionStarted",
            request.UserId,
            request.ItemId
        });

        await _kafka.PublishAsync("transactions", new
        {
            TransactionId = transactionId,
            request.UserId,
            request.ItemId
        });

        return Unit.Value;
    }

}

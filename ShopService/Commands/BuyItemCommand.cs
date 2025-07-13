using MediatR;
namespace ShopTestEventSourcingCqrsKafka.ShopService.Commands;

public record BuyItemCommand(Guid UserId, Guid ItemId) : IRequest<Unit>;

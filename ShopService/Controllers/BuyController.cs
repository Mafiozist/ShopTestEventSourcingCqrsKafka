using Microsoft.AspNetCore.Mvc;
using MediatR;
using ShopTestEventSourcingCqrsKafka.ShopService.Commands;

namespace ShopTestEventSourcingCqrsKafka.ShopService.Controllers;


[ApiController]
[Route("[controller]")]
public class BuyController : ControllerBase
{
    private readonly IMediator _mediator;
    public BuyController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Buy([FromBody] BuyItemCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok("Transaction started");
    }
}

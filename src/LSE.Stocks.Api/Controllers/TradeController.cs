using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Commands.SaveTrade;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LSE.Stocks.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TradeController : Controller
{
    private readonly IMediator _mediator;

    public TradeController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult> SaveTrade([FromBody] SaveTradeRequest tradeRequest)
    {
        await _mediator.Send(new SaveTradeCommand(tradeRequest.TickerSymbol, tradeRequest.Price,
            tradeRequest.Count, tradeRequest.BrokerId));

        return Ok();
    }
}

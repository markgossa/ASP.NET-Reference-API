using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Commands.SaveTrade;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LSE.Stocks.Api.Controllers.Common;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("[controller]")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class TradeController : Controller
{
    private readonly IMediator _mediator;

    public TradeController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Saves a trade of a share
    /// </summary>
    /// <param name="tradeRequest"></param>
    /// <returns>Ok</returns>
    /// <response code="200">Returns 200 if the trade was saved successfully</response>
    /// <response code="400">Returns 400 if the request to save a trade was invalid</response>
    [HttpPost]
    public async Task<ActionResult> SaveTrade([FromBody] SaveTradeRequest tradeRequest)
    {
        await _mediator.Send(new SaveTradeCommand(tradeRequest.TickerSymbol, tradeRequest.Price,
            tradeRequest.Count, tradeRequest.BrokerId));

        return Ok();
    }
}

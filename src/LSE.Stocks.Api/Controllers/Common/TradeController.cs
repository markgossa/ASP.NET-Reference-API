using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Commands.SaveTrade;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LSE.Stocks.Api.Controllers.Common;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("[controller]")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class TradesController : Controller
{
    private readonly IMediator _mediator;

    public TradesController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Saves a trade of a share
    /// </summary>
    /// <param name="tradeRequest"></param>
    /// <response code="201">Returns 201 CREATED if the trade was saved successfully</response>
    /// <response code="400">Returns 400 BAD REQUEST if the request to save a trade was invalid</response>
    [HttpPost]
    public async Task<ActionResult<SaveTradeResponse>> SaveTrade([FromBody] TradeRequest tradeRequest)
    {
        await _mediator.Send(MapToSaveTradeCommand(tradeRequest));

        return Created(string.Empty, GenerateSaveTradeResponse(tradeRequest));
    }

    private static SaveTradeCommand MapToSaveTradeCommand(TradeRequest tradeRequest)
        => new(tradeRequest.TickerSymbol, tradeRequest.Price, tradeRequest.Count, tradeRequest.BrokerId);

    private static SaveTradeResponse GenerateSaveTradeResponse(TradeRequest tradeRequest)
        => new(tradeRequest.TickerSymbol, tradeRequest.Price, tradeRequest.Count, tradeRequest.BrokerId);
}

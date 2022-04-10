using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Queries.GetSharePrice;
using LSE.Stocks.Domain.Models.Shares;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LSE.Stocks.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SharePriceController : Controller
{
    private readonly IMediator _mediator;

    public SharePriceController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<SharePriceResponse>> SaveShareExchange([FromQuery] string tickerSymbol)
    {
        var sharePriceQueryResponse = await _mediator.Send(new GetSharePriceQuery(tickerSymbol));

        return new OkObjectResult(BuildSharePriceQueryResponse(sharePriceQueryResponse.SharePrice));
    }

    private static SharePriceResponse BuildSharePriceQueryResponse(SharePrice sharePrice) 
        => new (sharePrice.TickerSymbol, sharePrice.Price);
}

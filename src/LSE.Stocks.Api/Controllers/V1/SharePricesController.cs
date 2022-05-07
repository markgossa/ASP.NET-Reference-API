using LSE.Stocks.Api.Models;
using LSE.Stocks.Api.Services;
using LSE.Stocks.Application.Services.Shares.Queries.GetSharePrice;
using LSE.Stocks.Domain.Models.Shares;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LSE.Stocks.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("SharePrices")]
[Route("v{version:apiVersion}/SharePrices")]
[ApiController]
public class SharePricesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICorrelationIdService _correlationIdService;

    public SharePricesController(IMediator mediator, ICorrelationIdService correlationIdService)
    {
        _mediator = mediator;
        _correlationIdService = correlationIdService;
    }

    /// <summary>
    /// Gets the price for a ticker symbol
    /// </summary>
    /// <param name="tickerSymbol"></param>
    /// <returns>A SharePriceResponse which contains the price of the share</returns>
    /// <response code="200">Returns 200 and the share price</response>
    /// <response code="400">Returns 400 if the query is invalid</response>
    [HttpGet]
    public async Task<ActionResult<SharePriceResponse>> GetPrice([FromQuery] string tickerSymbol)
    {
        var sharePriceQueryResponse = await _mediator.Send(new GetSharePriceQuery(tickerSymbol));
        AddCorrelationIdHeader();

        return new OkObjectResult(BuildSharePriceQueryResponse(sharePriceQueryResponse.SharePrice));
    }

    private void AddCorrelationIdHeader() 
        => Response.Headers.Add("Correlation-Id", new (_correlationIdService.CorrelationId));

    private static SharePriceResponse BuildSharePriceQueryResponse(SharePrice sharePrice)
        => new (sharePrice.TickerSymbol, sharePrice.Price);
}

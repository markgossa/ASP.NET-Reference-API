using LSE.Stocks.Api.Models;
using LSE.Stocks.Infrastructure.Models;
using LSE.Stocks.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using LSE.Stocks.Application.Exceptions;

namespace LSE.Stocks.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("SharePrices")]
[Route("v{version:apiVersion}/SharePrices")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class SharePricesController : Controller
{
    private readonly IGetSharePrice _sharePrice;

    public SharePricesController(IGetSharePrice sharePrice)
    {
        _sharePrice = sharePrice;
    }

    [HttpGet]
    public async Task<ActionResult<SharePriceResponse>> Price([FromQuery] string tickerSymbol)
    {
        if (string.IsNullOrWhiteSpace(tickerSymbol) || tickerSymbol.Length > 20)
        {
            return BadRequest();
        }

        return await _sharePrice.get(tickerSymbol) == null 
            ? throw new NotFoundException() 
            : (ActionResult<SharePriceResponse>?)await _sharePrice.get(tickerSymbol);
    }
}

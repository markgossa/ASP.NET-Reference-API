using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Queries.GetSharePrice;
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
        var response = await _mediator.Send(new GetSharePriceQuery(tickerSymbol));

        return new OkObjectResult(new SharePriceResponse(tickerSymbol, response.Price));
    }
}

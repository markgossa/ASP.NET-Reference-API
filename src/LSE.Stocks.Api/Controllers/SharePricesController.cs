using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Commands.SaveShareExchange;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LSE.Stocks.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SharePricesController : Controller
{
    private readonly IMediator _mediator;

    public SharePricesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<SharePriceResponse>> SaveShareExchange([FromQuery] string tickerSymbol)
    {
        //await _mediator.Send();

        return new OkObjectResult(new SharePriceResponse(tickerSymbol, 20));
    }
}

using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Services.Shares.Commands.SaveShareExchange;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LSE.Stocks.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShareExchangeController : Controller
    {
        private readonly IMediator _mediator;

        public ShareExchangeController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> SaveShareExchange([FromBody] SaveShareExchangeRequest shareExchangeRequest)
        {
            await _mediator.Send(new SaveShareExchangeCommand(shareExchangeRequest.TickerSymbol, shareExchangeRequest.Price,
                shareExchangeRequest.Count, shareExchangeRequest.BrokerId));

            return Ok();
        }
    }
}

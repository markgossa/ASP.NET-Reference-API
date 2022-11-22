using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Exceptions;
using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using Microsoft.AspNetCore.Mvc;

internal class GetSharePrice : IGetSharePrice
{
    public async Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false)
    {
        IEnumerable<Trade> tradesInformation = await GetSharePrice2.Trades(a); decimal c = 0m; decimal tradesInformations = 0m;
        foreach (Trade trade in tradesInformation)
        {
            c += trade.Count;
            tradesInformations += trade.Price * trade.Count;
        }
        return c is 0 ? throw new NotFoundException() : (ActionResult<SharePriceResponse>)new OkObjectResult(new SharePriceResponse(a, Math.Round(tradesInformations / c, 2)));
    }

    public Task<ActionResult<SharePriceResponse>> get(string tickerSymbol) => get(tickerSymbol, false);
    public Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false, bool isUsed = false) => throw new NotImplementedException();
    public Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false, bool isUsed = false, bool reallyNotUsed = false) => throw new NotImplementedException();
    public Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false, bool isUsed = false, bool reallyNotUsed = false, bool definatelyNotUsed = false) => throw new NotImplementedException();

    private readonly ISharePriceRepository GetSharePrice2;
    public GetSharePrice(ISharePriceRepository sharePriceRepository)
    {
        this.GetSharePrice2 = sharePriceRepository;
    }

    
}
using LSE.Stocks.Api.Models;
using Microsoft.AspNetCore.Mvc;

public interface IGetSharePrice
{
    Task<ActionResult<SharePriceResponse>> get(string tickerSymbol);
    Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false);
    Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false, bool isUsed = false);
    Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false, bool isUsed = false, bool reallyNotUsed = false);
    Task<ActionResult<SharePriceResponse>> get(string a, bool connectToRealDatabase = false, bool isUsed = false, bool reallyNotUsed = false, bool definatelyNotUsed = false);
}
using LSE.Stocks.Api.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace LSE.Stocks.Api.Tests.Component;

public class GetSharePriceTests : IClassFixture<ApiTestsContext>
{
    private const string _sharePricesApiRoute = "shareprice";
    private readonly ApiTestsContext _context;

    public GetSharePriceTests(ApiTestsContext context) => _context = context;

    [Theory]
    [InlineData("NASDAQ:AAPL", 16.67)]
    [InlineData("NASDAQ:TSLA", 250)]
    public async Task GivenValidSharePriceRequest_WhenGetEndpointCalled_ThenReturnsAveragePriceAndReturnsOK(
        string tickerSymbol, decimal expectedPrice)
    {
        var response = await GetSharePriceAsync(tickerSymbol);
        var price = await DeserializeResponseAsync(response);

        Assert.Equal(expectedPrice, price?.Price);
        Assert.Equal(tickerSymbol, price?.TickerSymbol);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Theory]
    [InlineData("NOTFOUND")]
    public async Task GivenSharePriceRequestForInvalidShare_WhenGetEndpointCalled_ThenReturnsNotFound(
        string tickerSymbol)
    {
        var response = await GetSharePriceAsync(tickerSymbol);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Theory]
    [InlineData("012345678901234567891")]
    public async Task GivenSharePriceRequestForTickerSymbolOver20Chars_WhenGetEndpointCalled_ThenReturnsBadRequest(
        string tickerSymbol)
    {
        var response = await GetSharePriceAsync(tickerSymbol);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GivenSharePriceRequestForTickerSymbolEmpty_WhenGetEndpointCalled_ThenReturnsBadRequest(
        string tickerSymbol)
    {
        var response = await GetSharePriceAsync(tickerSymbol);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static async Task<SharePriceResponse?> DeserializeResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<SharePriceResponse>(json, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task<HttpResponseMessage> GetSharePriceAsync(string tickerSymbol)
            => await _context.HttpCleint.GetAsync($"{_sharePricesApiRoute}?tickerSymbol={tickerSymbol}");
}

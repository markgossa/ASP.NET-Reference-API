using LSE.Stocks.Api.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace LSE.Stocks.Api.Tests.Component;

public class GetSharePriceTests : IClassFixture<ApiTestsContext>
{
    private const string _sharePricesApiRoute = "shareprices";
    private readonly ApiTestsContext _context;

    public GetSharePriceTests(ApiTestsContext context) => _context = context;

    [Theory]
    [InlineData("NASDAQ:AAPL", 20)]
    //[InlineData("NASDAQ:TSLA")]
    public async Task GivenValidSharePriceRequest_WhenGetEndpointCalled_ThenReturnsAveragePriceAndReturnsOK(
        string tickerSymbol, int expectedPrice)
    {
        var response = await GetSharePriceAsync(tickerSymbol);
        var price = await DeserializeResponseAsync(response);

        Assert.Equal(expectedPrice, price?.Price);
        Assert.Equal(tickerSymbol, price?.TickerSymbol);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

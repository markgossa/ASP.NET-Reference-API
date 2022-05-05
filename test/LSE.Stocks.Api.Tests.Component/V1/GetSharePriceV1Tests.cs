﻿using LSE.Stocks.Api.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace LSE.Stocks.Api.Tests.Component.V1;

public class GetSharePriceV1Tests : IClassFixture<ApiTestsContext>
{
    private const string _apiRoute = "shareprices";
    private const string _apiRouteV1 = "v1/shareprices";
    private readonly ApiTestsContext _context;

    public GetSharePriceV1Tests(ApiTestsContext context) => _context = context;

    [Theory]
    [InlineData("NASDAQ:AAPL", 16.67, _apiRoute)]
    [InlineData("NASDAQ:TSLA", 250, _apiRouteV1)]
    public async Task GivenValidSharePriceRequest_WhenGetEndpointCalled_ThenReturnsAveragePriceAndReturnsOK(
        string tickerSymbol, decimal expectedPrice, string apiRoute)
    {
        var response = await GetSharePriceAsync(tickerSymbol, apiRoute);
        var price = await DeserializeResponseAsync(response);

        Assert.Equal(expectedPrice, price?.Price);
        Assert.Equal(tickerSymbol, price?.TickerSymbol);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("NOTFOUND", _apiRoute)]
    [InlineData("NOTFOUND", _apiRouteV1)]
    public async Task GivenSharePriceRequestForInvalidShare_WhenGetEndpointCalled_ThenReturnsNotFound(
        string tickerSymbol, string apiRoute)
    {
        var response = await GetSharePriceAsync(tickerSymbol, apiRoute);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("012345678901234567891", _apiRoute)]
    [InlineData("012345678901234567891", _apiRouteV1)]
    public async Task GivenSharePriceRequestForTickerSymbolOver20Chars_WhenGetEndpointCalled_ThenReturnsBadRequest(
        string tickerSymbol, string apiRoute)
    {
        var response = await GetSharePriceAsync(tickerSymbol, apiRoute);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("", _apiRoute)]
    [InlineData(" ", _apiRoute)]
    [InlineData(null, _apiRoute)]
    [InlineData("", _apiRouteV1)]
    [InlineData(" ", _apiRouteV1)]
    [InlineData(null, _apiRouteV1)]
    public async Task GivenSharePriceRequestForTickerSymbolEmpty_WhenGetEndpointCalled_ThenReturnsBadRequest(
        string tickerSymbol, string apiRoute)
    {
        var response = await GetSharePriceAsync(tickerSymbol, apiRoute);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("NASDAQ:ERROR", _apiRoute)]
    [InlineData("NASDAQ:ERROR", _apiRouteV1)]
    public async Task GivenSharePriceReositoryHasError_WhenGetEndpointCalled_ThenReturnsInternalServerError(
        string tickerSymbol, string apiRoute)
    {
        var response = await GetSharePriceAsync(tickerSymbol, apiRoute);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    private static async Task<SharePriceResponse?> DeserializeResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<SharePriceResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task<HttpResponseMessage> GetSharePriceAsync(string tickerSymbol, string apiRoute)
            => await _context.HttpClient.GetAsync($"{apiRoute}?tickerSymbol={tickerSymbol}");
}
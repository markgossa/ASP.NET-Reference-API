﻿using LSE.Stocks.Api.Models;
using LSE.Stocks.Domain.Models.Shares;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Xunit;

namespace LSE.Stocks.Api.Tests.Component;

public class SaveTradeTests : IClassFixture<ApiTestsContext>
{
    private const string _tradeApiRoute = "trade";
    private readonly ApiTestsContext _context;

    public SaveTradeTests(ApiTestsContext context) => _context = context;

    [Theory]
    [InlineData("NASDAQ:AAPL", 10, 1, "BR10834")]
    [InlineData("NASDAQ:TSLA", 25.05, 2, "BR00432")]
    public async Task GivenValidTradeRequest_WhenPostEndpointCalled_ThenSavesTradeAndReturnsOK(
        string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var tradeRequest = new SaveTradeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostTradeAsync(tradeRequest);

        AssertTradeSaved(tradeRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("012345678901234567891", 10, 1, "BR10834")]
    public async Task GivenValidTradeRequestWithTickerSymbolOver20Chars_WhenPostEndpointCalled_ThenDoesNotTradeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId) 
            => await PostTradeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);
    
    [Theory]
    [InlineData("", 10, 1, "BR10834")]
    [InlineData(" ", 10, 1, "BR10834")]
    [InlineData(null, 10, 1, "BR10834")]
    public async Task GivenValidTradeRequestWithEmptyTickerSymbol_WhenPostEndpointCalled_ThenDoesNotTradeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId) 
            => await PostTradeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);

    [Theory]
    [InlineData("NASDAQ:AAPL", 0, 1, "BR10834")]
    [InlineData("NASDAQ:AAPL", -1, 1, "BR10834")]
    public async Task GivenValidTradeRequestWithPriceZeroOrLess_WhenPostEndpointCalled_ThenDoesNotTradeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
            => await PostTradeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);

    [Theory]
    [InlineData("NASDAQ:AAPL", 150, 0, "BR10834")]
    [InlineData("NASDAQ:AAPL", 200, -1, "BR10834")]
    public async Task GivenValidTradeRequestWithCountZeroOrLess_WhenPostEndpointCalled_ThenDoesNotTradeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
            => await PostTradeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);

    private async Task<HttpResponseMessage> PostTradeAsync(SaveTradeRequest tradeRequest)
            => await _context.HttpCleint.PostAsync(_tradeApiRoute, BuildHttpContent(tradeRequest));

    private static StringContent BuildHttpContent(SaveTradeRequest tradeRequest)
        => new(JsonSerializer.Serialize(tradeRequest), Encoding.UTF8, MediaTypeNames.Application.Json);

    private void AssertTradeSaved(SaveTradeRequest tradeRequest)
            => _context.MockTradeRepository.Verify(m => m.SaveTradeAsync(MapToTrade(tradeRequest)),
                Times.Once);

    private static Trade MapToTrade(SaveTradeRequest saveTradeRequest)
        => new(saveTradeRequest.TickerSymbol, saveTradeRequest.Price,
            saveTradeRequest.Count, saveTradeRequest.BrokerId);

    private void AssertTradeNotSaved(SaveTradeRequest tradeRequest)
            => _context.MockTradeRepository.Verify(m => m.SaveTradeAsync(MapToTrade(tradeRequest)),
                Times.Never);

    private async Task PostTradeAndAssertNotSavedAndBadRequest(string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var tradeRequest = new SaveTradeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostTradeAsync(tradeRequest);

        AssertTradeNotSaved(tradeRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
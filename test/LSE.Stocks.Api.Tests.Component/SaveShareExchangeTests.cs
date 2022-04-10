using LSE.Stocks.Api.Models;
using LSE.Stocks.Domain.Models.Shares;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Xunit;

namespace LSE.Stocks.Api.Tests.Component;

public class SaveShareExchangeTests : IClassFixture<ApiTestsContext>
{
    private const string _shareExchangeApiRoute = "shareexchange";
    private readonly ApiTestsContext _context;

    public SaveShareExchangeTests(ApiTestsContext context) => _context = context;

    [Theory]
    [InlineData("NASDAQ:AAPL", 10, 1, "BR10834")]
    [InlineData("NASDAQ:TSLA", 25.05, 2, "BR00432")]
    public async Task GivenValidShareExchangeRequest_WhenPostEndpointCalled_ThenSavesShareExchangeAndReturnsOK(
        string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostShareExchangeAsync(shareExchangeRequest);

        AssertShareExchangeSaved(shareExchangeRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("012345678901234567891", 10, 1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithTickerSymbolOver20Chars_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId) 
            => await PostShareExchangeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);
    
    [Theory]
    [InlineData("", 10, 1, "BR10834")]
    [InlineData(" ", 10, 1, "BR10834")]
    [InlineData(null, 10, 1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithEmptyTickerSymbol_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId) 
            => await PostShareExchangeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);

    [Theory]
    [InlineData("NASDAQ:AAPL", 0, 1, "BR10834")]
    [InlineData("NASDAQ:AAPL", -1, 1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithPriceZeroOrLess_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
            => await PostShareExchangeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);

    [Theory]
    [InlineData("NASDAQ:AAPL", 150, 0, "BR10834")]
    [InlineData("NASDAQ:AAPL", 200, -1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithCountZeroOrLess_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
            => await PostShareExchangeAndAssertNotSavedAndBadRequest(tickerSymbol, price, count, brokerId);

    private async Task<HttpResponseMessage> PostShareExchangeAsync(SaveShareExchangeRequest shareExchangeRequest)
            => await _context.HttpCleint.PostAsync(_shareExchangeApiRoute, BuildHttpContent(shareExchangeRequest));

    private static StringContent BuildHttpContent(SaveShareExchangeRequest shareExchangeRequest)
        => new(JsonSerializer.Serialize(shareExchangeRequest), Encoding.UTF8, MediaTypeNames.Application.Json);

    private void AssertShareExchangeSaved(SaveShareExchangeRequest shareExchangeRequest)
            => _context.MockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(MapToShareExchange(shareExchangeRequest)),
                Times.Once);

    private static ShareExchange MapToShareExchange(SaveShareExchangeRequest saveShareExchangeRequest)
        => new(saveShareExchangeRequest.TickerSymbol, saveShareExchangeRequest.Price,
            saveShareExchangeRequest.Count, saveShareExchangeRequest.BrokerId);

    private void AssertShareExchangeNotSaved(SaveShareExchangeRequest shareExchangeRequest)
            => _context.MockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(MapToShareExchange(shareExchangeRequest)),
                Times.Never);

    private async Task PostShareExchangeAndAssertNotSavedAndBadRequest(string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostShareExchangeAsync(shareExchangeRequest);

        AssertShareExchangeNotSaved(shareExchangeRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

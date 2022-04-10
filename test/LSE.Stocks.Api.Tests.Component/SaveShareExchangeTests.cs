using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Xunit;

namespace LSE.Stocks.Api.Tests.Component;

public class SaveShareExchangeTests
{
    private const string _shareExchangeApiRoute = "shareexchange";

    [Theory]
    [InlineData("NASDAQ:AAPL", 10, 1, "BR10834")]
    [InlineData("NASDAQ:TSLA", 25.05, 2, "BR00432")]
    public async Task GivenValidShareExchangeRequest_WhenPostEndpointCalled_ThenSavesShareExchangeAndReturnsOK(
        string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var mockShareExchangeRepository = new Mock<IShareExchangeRepository>();

        var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostShareExchangeAsync(mockShareExchangeRepository.Object, shareExchangeRequest);

        AssertShareExchangeSaved(mockShareExchangeRepository, shareExchangeRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Theory]
    [InlineData("012345678901234567891", 10, 1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithTickerSymbolOver20Chars_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var mockShareExchangeRepository = new Mock<IShareExchangeRepository>();

        var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostShareExchangeAsync(mockShareExchangeRepository.Object, shareExchangeRequest);

        AssertShareExchangeNotSaved(mockShareExchangeRepository, shareExchangeRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData("NASDAQ:AAPL", 0, 1, "BR10834")]
    [InlineData("NASDAQ:AAPL", -1, 1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithPriceZeroOrLess_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var mockShareExchangeRepository = new Mock<IShareExchangeRepository>();

        var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostShareExchangeAsync(mockShareExchangeRepository.Object, shareExchangeRequest);

        AssertShareExchangeNotSaved(mockShareExchangeRepository, shareExchangeRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData("NASDAQ:AAPL", 150, 0, "BR10834")]
    [InlineData("NASDAQ:AAPL", 200, -1, "BR10834")]
    public async Task GivenValidShareExchangeRequestWithCountZeroOrLess_WhenPostEndpointCalled_ThenDoesNotShareExchangeAndReturnsBadRequest(
        string tickerSymbol, decimal price, decimal count, string brokerId)
    {
        var mockShareExchangeRepository = new Mock<IShareExchangeRepository>();

        var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
        var response = await PostShareExchangeAsync(mockShareExchangeRepository.Object, shareExchangeRequest);

        AssertShareExchangeNotSaved(mockShareExchangeRepository, shareExchangeRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static async Task<HttpResponseMessage> PostShareExchangeAsync(IShareExchangeRepository shareExchangeRepository,
        SaveShareExchangeRequest shareExchangeRequest) 
            => await BuildClient(shareExchangeRepository).PostAsync(_shareExchangeApiRoute, BuildHttpContent(shareExchangeRequest));

    private static HttpClient BuildClient(IShareExchangeRepository shareExchangeRepository) 
        => new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(b => b.ConfigureServices(services
                => ((ServiceCollection)services).AddSingleton(shareExchangeRepository)))
                    .CreateClient();

    private static StringContent BuildHttpContent(SaveShareExchangeRequest shareExchangeRequest)
        => new(JsonSerializer.Serialize(shareExchangeRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
    
    private static void AssertShareExchangeSaved(Mock<IShareExchangeRepository> mockShareExchangeRepository, 
        SaveShareExchangeRequest shareExchangeRequest)
            => mockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(MapToShareExchange(shareExchangeRequest)),
                Times.Once);

    private static ShareExchange MapToShareExchange(SaveShareExchangeRequest saveShareExchangeRequest)
        => new(saveShareExchangeRequest.TickerSymbol, saveShareExchangeRequest.Price,
            saveShareExchangeRequest.Count, saveShareExchangeRequest.BrokerId);

    private static void AssertShareExchangeNotSaved(Mock<IShareExchangeRepository> mockShareExchangeRepository, 
        SaveShareExchangeRequest shareExchangeRequest)
            => mockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(MapToShareExchange(shareExchangeRequest)),
                Times.Never);
}

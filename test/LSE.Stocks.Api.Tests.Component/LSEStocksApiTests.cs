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

namespace LSE.Stocks.Api.Tests.Component
{
    public class LSEStocksApiTests
    {
        private const string _shareExchangeApiRoute = "shareexchange";

        [Theory]
        [InlineData("NASDAQ:AAPL", 10, 1, "BR10834")]
        [InlineData("NASDAQ:TSLA", 25.05, 2, "BR00432")]
        public async Task GivenValidShareExchange_WhenPostEndpointCalled_ThenSavesShareExchangeAndReturnsOK(
            string tickerSymbol, decimal price, decimal count, string brokerId)
        {
            var mockShareExchangeRepository = new Mock<IShareExchangeRepository>();

            var shareExchangeRequest = new SaveShareExchangeRequest(tickerSymbol, price, count, brokerId);
            var response = await PostShareExchangeAsync(mockShareExchangeRepository.Object, shareExchangeRequest);

            AssertShareExchangeSaved(mockShareExchangeRepository, shareExchangeRequest);
            AssertOk(response.StatusCode);
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
        
        private static void AssertShareExchangeSaved(Mock<IShareExchangeRepository> mockShareExchangeRepository, SaveShareExchangeRequest shareExchangeRequest)
            => mockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(MapToShareExchange(shareExchangeRequest)));

        private static ShareExchange MapToShareExchange(SaveShareExchangeRequest saveShareExchangeRequest)
            => new(saveShareExchangeRequest.TickerSymbol, saveShareExchangeRequest.Price,
                saveShareExchangeRequest.Count, saveShareExchangeRequest.BrokerId);

        private static void AssertOk(HttpStatusCode httpStatusCode) => Assert.Equal(HttpStatusCode.OK, httpStatusCode);
    }
}

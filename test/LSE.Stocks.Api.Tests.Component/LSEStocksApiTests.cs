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

        [Fact]
        public async Task GivenValidShareExchange_WhenPostEndpointCalled_ThenSavesShareExchangeAndReturnsOK()
        {
            var mockShareExchangeRepository = new Mock<IShareExchangeRepository>();
            var client = BuildClient(mockShareExchangeRepository);

            var shareExchangeRequest = new SaveShareExchangeRequest("NSADAQ:AAPL", 10.00m, 1m, Guid.NewGuid().ToString());
            var response = await client.PostAsync(_shareExchangeApiRoute, BuildHttpContent(shareExchangeRequest));

            mockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(MapToShareExchange(shareExchangeRequest)));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private static HttpClient BuildClient(Mock<IShareExchangeRepository> mockShareExchangeRepository) 
            => new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(b => b.ConfigureServices(services
                    => ((ServiceCollection)services).AddSingleton(mockShareExchangeRepository.Object)))
                        .CreateClient();

        private static ShareExchange MapToShareExchange(SaveShareExchangeRequest saveShareExchangeRequest)
            => new(saveShareExchangeRequest.TickerSymbol, saveShareExchangeRequest.Price,
                saveShareExchangeRequest.Count, saveShareExchangeRequest.BrokerId);

        private static StringContent BuildHttpContent(SaveShareExchangeRequest shareExchangeRequest)
            => new(JsonSerializer.Serialize(shareExchangeRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}

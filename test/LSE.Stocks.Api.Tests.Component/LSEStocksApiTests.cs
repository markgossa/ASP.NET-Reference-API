using LSE.Stocks.Api.Models;
using LSE.Stocks.Application.Repositories;
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
            var webApplicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(b => b.ConfigureServices(services 
                    => ((ServiceCollection)services).AddSingleton(mockShareExchangeRepository.Object)));

            var client = webApplicationFactory.CreateClient();

            var shareExchangeRequest = new SaveShareExchangeRequest("NSADAQ:AAPL", 10.00m, 1m, Guid.NewGuid().ToString());

            var response = await client.PostAsync(_shareExchangeApiRoute, BuildHttpContent(shareExchangeRequest));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            mockShareExchangeRepository.Verify(m => m.SaveShareExchangeAsync(shareExchangeRequest.TickerSymbol,
                shareExchangeRequest.Price, shareExchangeRequest.Count, shareExchangeRequest.BrokerId), Times.Once);
        }

        private static StringContent BuildHttpContent(SaveShareExchangeRequest shareExchangeRequest)
            => new(JsonSerializer.Serialize(shareExchangeRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}

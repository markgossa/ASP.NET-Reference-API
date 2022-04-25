using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;

namespace LSE.Stocks.Api.Tests.Component
{
    public class ApiTestsContext : WebApplicationFactory<Startup>, IDisposable
    {
        public Mock<ITradeRepository> MockTradeRepository { get; } = new();
        public HttpClient HttpClient { get; }
        private readonly Mock<ISharePriceRepository> _mockSharePriceRepository = new();

        public ApiTestsContext()
        {
            HttpClient = CreateClient();
            SetUpMockSharePricingRepository();
        }

        private void SetUpMockSharePricingRepository()
        {
            _mockSharePriceRepository.Setup(m => m.GetTradesAsync("NASDAQ:AAPL"))
                 .ReturnsAsync(new List<Trade>()
                    {
                        new("NASDAQ:AAPL", 10, 2, null),
                        new("NASDAQ:AAPL", 20, 4, null),
                    });

            _mockSharePriceRepository.Setup(m => m.GetTradesAsync("NASDAQ:TSLA"))
                 .ReturnsAsync(new List<Trade>()
                    {
                        new("NASDAQ:TSLA", 150, 2, null),
                        new("NASDAQ:TSLA", 300, 4, null)
                    });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
            => builder.ConfigureServices(services =>
            {
                ((ServiceCollection)services).AddSingleton(MockTradeRepository.Object);
                ((ServiceCollection)services).AddSingleton(_mockSharePriceRepository.Object);
            });

        public new void Dispose()
        {
            HttpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;

namespace LSE.Stocks.Api.Tests.Component
{
    public class ApiTestsContext : IDisposable
    {
        public Mock<IShareExchangeRepository> MockShareExchangeRepository { get; } = new();
        public HttpClient HttpCleint { get; }
        private readonly Mock<ISharePriceRepository> _mockSharePriceRepository = new();
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public ApiTestsContext()
        {
            _webApplicationFactory = BuildWebApplicationFactory();
            HttpCleint = _webApplicationFactory.CreateClient();
            SetUpMockSharePricingRepository();
        }

        private void SetUpMockSharePricingRepository()
        {
            _mockSharePriceRepository.Setup(m => m.GetShareExchangesAsync("NASDAQ:AAPL"))
                 .ReturnsAsync(new List<ShareExchange>()
                    {
                        new("NASDAQ:AAPL", 10, 2, null),
                        new("NASDAQ:AAPL", 20, 4, null),
                    });
            
            _mockSharePriceRepository.Setup(m => m.GetShareExchangesAsync("NASDAQ:TSLA"))
                 .ReturnsAsync(new List<ShareExchange>()
                    {
                        new("NASDAQ:TSLA", 150, 2, null),
                        new("NASDAQ:TSLA", 300, 4, null)
                    });
        }

        private WebApplicationFactory<Startup> BuildWebApplicationFactory()
            => new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(b => b.ConfigureServices(services =>
            {
                ((ServiceCollection)services).AddSingleton(MockShareExchangeRepository.Object);
                ((ServiceCollection)services).AddSingleton(_mockSharePriceRepository.Object);
            }));

        public void Dispose()
        {
            HttpCleint.Dispose();
            _webApplicationFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
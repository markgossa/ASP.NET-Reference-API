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
        private readonly Mock<ISharePricingRepository> _mockSharePricingRepository = new();
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public ApiTestsContext()
        {
            _webApplicationFactory = BuildWebApplicationFactory();
            HttpCleint = _webApplicationFactory.CreateClient();
            SetUpMockSharePricingRepository();
        }

        private void SetUpMockSharePricingRepository()
            => _mockSharePricingRepository.Setup(m => m.GetSharePricesAsync("NASDAQ:AAPL"))
                .ReturnsAsync(new List<SharePrice>()
                    {
                        new("NASDAQ:AAPL", 10, 1),
                        new("NASDAQ:AAPL", 20, 1),
                    });

        private WebApplicationFactory<Startup> BuildWebApplicationFactory()
            => new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(b => b.ConfigureServices(services =>
            {
                ((ServiceCollection)services).AddSingleton(MockShareExchangeRepository.Object);
                ((ServiceCollection)services).AddSingleton(_mockSharePricingRepository.Object);
            }));

        public void Dispose()
        {
            HttpCleint.Dispose();
            _webApplicationFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
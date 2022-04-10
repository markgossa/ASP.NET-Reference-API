using LSE.Stocks.Application.Repositories;
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
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public ApiTestsContext()
        {
            _webApplicationFactory = BuildWebApplicationFactory();
            HttpCleint = _webApplicationFactory.CreateClient();
        }

        private WebApplicationFactory<Startup> BuildWebApplicationFactory()
            => new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(b => b.ConfigureServices(services
                => ((ServiceCollection)services).AddSingleton(MockShareExchangeRepository.Object)));

        public void Dispose()
        {
            HttpCleint.Dispose();
            _webApplicationFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
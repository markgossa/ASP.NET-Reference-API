using LSE.Stocks.Application.Services.Shares.Commands.SaveShareExchange;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LSE.Stocks.Api.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(SaveShareExchangeCommand));

            return services;
        }
    }
}

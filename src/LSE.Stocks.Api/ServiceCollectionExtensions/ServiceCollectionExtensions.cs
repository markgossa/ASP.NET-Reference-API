using FluentValidation;
using LSE.Stocks.Application.Common.Behaviours;
using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Application.Services.Shares.Commands.SaveTrade;
using LSE.Stocks.Application.Services.Shares.Queries.GetSharePrice;
using LSE.Stocks.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LSE.Stocks.Api.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(GetSharePriceQueryValidator).Assembly);
        services.AddMediatR(typeof(SaveTradeCommand));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
        services.AddScoped<ITradeRepository, TradeSqlRepository>();
        services.AddScoped<ISharePriceRepository, SharePriceSqlRepository>();

        return services;
    }
}

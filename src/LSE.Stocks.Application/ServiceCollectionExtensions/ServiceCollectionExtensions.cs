using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LSE.Stocks.Application.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

using LSE.Stocks.Api.Services;
using Microsoft.AspNetCore.Http;

namespace LSE.Stocks.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ICorrelationIdService correlationIdService)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-Correlation-Id", new[] { correlationIdService.CorrelationId });
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}

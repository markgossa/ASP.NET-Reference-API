using MediatR;

namespace LSE.Stocks.Application.Services.Shares.Commands.SaveShareExchange
{
    public record SaveShareExchangeCommand(string TickerSymbol, decimal Price, decimal Count, string BrokerId) : IRequest;
}
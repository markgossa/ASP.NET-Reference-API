namespace LSE.Stocks.Api.Models;

public record SaveShareExchangeRequest(string TickerSymbol, decimal Price, decimal Count, string BrokerId);

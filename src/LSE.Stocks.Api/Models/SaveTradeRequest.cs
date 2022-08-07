namespace LSE.Stocks.Api.Models;

public record TradeRequest(string TickerSymbol, decimal Price, decimal Count, string BrokerId);

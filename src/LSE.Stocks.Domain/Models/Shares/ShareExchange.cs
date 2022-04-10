namespace LSE.Stocks.Domain.Models.Shares;

public record ShareExchange(string TickerSymbol, decimal Price, decimal Count, string BrokerId);

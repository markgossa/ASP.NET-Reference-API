namespace LSE.Stocks.Application.Repositories
{
    public interface IShareExchangeRepository
    {
        Task SaveShareExchangeAsync(string tickerSymbol, decimal price, decimal count, string brokerId);
    }
}

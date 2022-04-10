using LSE.Stocks.Domain.Models.Shares;

namespace LSE.Stocks.Application.Repositories
{
    public interface ISharePricingRepository
    {
        Task<IEnumerable<SharePrice>> GetSharePricesAsync(string tickerSymbol);
    }
}

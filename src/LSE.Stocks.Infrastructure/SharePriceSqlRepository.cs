using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;

namespace LSE.Stocks.Infrastructure;

public class SharePriceSqlRepository : ISharePriceRepository
{
    public Task<IEnumerable<Trade>> GetTradesAsync(string tickerSymbol) => throw new NotImplementedException();
}

using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using LSE.Stocks.Infrastructure.Models;

namespace LSE.Stocks.Infrastructure;

public class SharePriceSqlRepository : ISharePriceRepository
{
    private readonly TradesDbContext _dbContext;

    public SharePriceSqlRepository(TradesDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Trade>> GetTradesAsync(string tickerSymbol)
        => _dbContext.Trades.Where(t => t.TickerSymbol.ToLower() == tickerSymbol.ToLower())
            .Select(t => new Trade(t.TickerSymbol, t.Price, t.Count, t.BrokerId));
}

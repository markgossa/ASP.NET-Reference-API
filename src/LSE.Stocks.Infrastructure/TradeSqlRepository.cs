using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using LSE.Stocks.Infrastructure.Models;

namespace LSE.Stocks.Infrastructure;

public class TradeSqlRepository : ITradeRepository
{
    private readonly TradesDbContext _dbContext;

    public TradeSqlRepository(TradesDbContext dbContext) => _dbContext = dbContext;

    public async Task SaveTradeAsync(Trade trade)
    {
        _dbContext.Add(new TradeRow()
        {
            TickerSymbol = trade.TickerSymbol,
            BrokerId = trade.BrokerId,
            Count = trade.Count,
            Price = trade.Price
        });

        await _dbContext.SaveChangesAsync();
    }
}

using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;

namespace LSE.Stocks.Infrastructure;

public class TradeSqlRepository : ITradeRepository
{
    public async Task SaveTradeAsync(Trade trade) => throw new NotImplementedException();
}

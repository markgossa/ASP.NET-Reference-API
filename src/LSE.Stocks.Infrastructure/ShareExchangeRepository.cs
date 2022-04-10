using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;

namespace LSE.Stocks.Infrastructure;

public class ShareExchangeRepository : IShareExchangeRepository
{
    public async Task SaveShareExchangeAsync(ShareExchange shareExchange) { }
}

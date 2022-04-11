using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using LSE.Stocks.Infrastructure.Models;

namespace LSE.Stocks.Infrastructure;

public class TradeSqlRepository : ITradeRepository, IDisposable, IAsyncDisposable
{
    private TradesDbContext? _dbContext;
    private bool _disposedValue;

    public TradeSqlRepository(TradesDbContext dbContext) => _dbContext = dbContext;

    public async Task SaveTradeAsync(Trade trade)
    {
        _dbContext!.Add(new TradeRow()
        {
            TickerSymbol = trade.TickerSymbol,
            BrokerId = trade.BrokerId,
            Count = trade.Count,
            Price = trade.Price
        });

        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
            }

            _dbContext = null;
            _disposedValue = true;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_dbContext is not null)
        {
            await _dbContext.DisposeAsync().ConfigureAwait(false);
        }

        _dbContext = null;
    }
}

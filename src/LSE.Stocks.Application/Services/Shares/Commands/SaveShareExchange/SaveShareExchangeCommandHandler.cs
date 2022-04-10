using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using MediatR;

namespace LSE.Stocks.Application.Services.Shares.Commands.SaveShareExchange;

internal class SaveShareExchangeCommandHandler : IRequestHandler<SaveShareExchangeCommand>
{
    private readonly IShareExchangeRepository _shareExchangeRepository;

    public SaveShareExchangeCommandHandler(IShareExchangeRepository shareExchangeRepository)
        => _shareExchangeRepository = shareExchangeRepository;

    public async Task<Unit> Handle(SaveShareExchangeCommand saveShareExchangeCommand, CancellationToken cancellationToken)
    {
        await _shareExchangeRepository.SaveShareExchangeAsync(MapToShareExchange(saveShareExchangeCommand));

        return Unit.Value;
    }

    private static ShareExchange MapToShareExchange(SaveShareExchangeCommand saveShareExchangeCommand) 
        => new (saveShareExchangeCommand.TickerSymbol, saveShareExchangeCommand.Price,
            saveShareExchangeCommand.Count, saveShareExchangeCommand.BrokerId);
}

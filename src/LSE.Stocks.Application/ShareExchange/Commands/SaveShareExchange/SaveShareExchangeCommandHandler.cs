using LSE.Stocks.Application.Repositories;
using MediatR;

namespace LSE.Stocks.Application.ShareExchange.Commands.SaveShareExchange
{
    internal class SaveShareExchangeCommandHandler : IRequestHandler<SaveShareExchangeCommand>
    {
        private readonly IShareExchangeRepository _shareExchangeRepository;

        public SaveShareExchangeCommandHandler(IShareExchangeRepository shareExchangeRepository) 
            => _shareExchangeRepository = shareExchangeRepository;

        public async Task<Unit> Handle(SaveShareExchangeCommand saveShareExchangeCommand, CancellationToken cancellationToken)
        {
            await _shareExchangeRepository.SaveShareExchangeAsync(saveShareExchangeCommand.TickerSymbol,
                saveShareExchangeCommand.Price, saveShareExchangeCommand.Count, saveShareExchangeCommand.BrokerId);

            return Unit.Value;
        }
    }
}

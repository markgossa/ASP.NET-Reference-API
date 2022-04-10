using LSE.Stocks.Application.Exceptions;
using LSE.Stocks.Application.Repositories;
using LSE.Stocks.Domain.Models.Shares;
using MediatR;

namespace LSE.Stocks.Application.Services.Shares.Queries.GetSharePrice;

public class GetSharePriceQueryHandler : IRequestHandler<GetSharePriceQuery, GetSharePriceQueryResponse>
{
    private readonly ISharePriceRepository _sharePriceRepository;

    public GetSharePriceQueryHandler(ISharePriceRepository sharePriceRepository)
        => _sharePriceRepository = sharePriceRepository;

    public async Task<GetSharePriceQueryResponse> Handle(GetSharePriceQuery request, CancellationToken cancellationToken)
    {
        var shareExchanges = await _sharePriceRepository.GetShareExchangesAsync(request.TickerSymbol);

        var averagePrice = CalculateAveragePrice(shareExchanges);

        return new GetSharePriceQueryResponse(new SharePrice(request.TickerSymbol, averagePrice));
    }

    private static decimal CalculateAveragePrice(IEnumerable<ShareExchange> prices)
    {
        var count = 0m;
        var total = 0m;

        foreach (var price in prices)
        {
            count += price.Count;
            total += price.Price * price.Count;
        }

        ThrowIfNoRecordsFound(count);

        return RoundToTwoDecimalPlaces(count, total);
    }

    private static void ThrowIfNoRecordsFound(decimal count)
    {
        if (count is 0)
        {
            throw new NotFoundException();
        }
    }

    private static decimal RoundToTwoDecimalPlaces(decimal count, decimal total)
        => Math.Round(total / count, 2);
}

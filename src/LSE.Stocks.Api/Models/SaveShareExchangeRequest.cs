namespace LSE.Stocks.Api.Models
{
    public class SaveShareExchangeRequest
    {
        public string TickerSymbol { get; }
        public decimal Price { get; }
        public decimal Count { get; }
        public string BrokerId { get; }

        public SaveShareExchangeRequest(string tickerSymbol, decimal price, decimal count, string brokerId)
        {
            TickerSymbol = tickerSymbol;
            Price = price;
            Count = count;
            BrokerId = brokerId;
        }
    }
}

namespace LSE.Stocks.Api.Services
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        public string Generate() => Guid.NewGuid().ToString();
    }
}

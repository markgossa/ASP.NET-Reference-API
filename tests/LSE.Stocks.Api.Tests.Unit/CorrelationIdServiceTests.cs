using LSE.Stocks.Api.Services;
using Xunit;

namespace LSE.Stocks.Api.Tests.Unit;

public class CorrelationIdServiceTests
{
    [Fact]
    public void GivenNewInstance_WhenIGetANewCorrelationId_ThenIGetANewGuid()
    {
        var sut = new CorrelationIdService();

        Assert.True(Guid.TryParse(sut.CorrelationId, out _));
    }

    [Fact]
    public void GivenNewInstance_WhenIGetANewCorrelationIdTwice_ThenIGetANewGuid_AndTheSecondGuidIsTheSame()
    {
        var sut = new CorrelationIdService();
        var isValidGuid1 = Guid.TryParse(sut.CorrelationId, out var correlationId1);
        var isValidGuid2 = Guid.TryParse(sut.CorrelationId, out var correlationId2);

        Assert.True(isValidGuid1);
        Assert.True(isValidGuid2);
        Assert.Equal(correlationId1, correlationId2);
    }

    [Fact]
    public void GivenNewInstance_WhenISetTheCorrelationId_ThenItIsReturned()
    {
        var sut = new CorrelationIdService();
        var expectedCorrelationId = Guid.NewGuid().ToString();
        sut.CorrelationId = expectedCorrelationId;

        Assert.Equal(expectedCorrelationId, sut.CorrelationId);
    }
}
using LSE.Stocks.Api.Middleware;
using LSE.Stocks.Api.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace LSE.Stocks.Api.Tests.Unit;
public class CorrelationIdMiddlewareTests
{
    private static readonly string _correlationIdHeader = "X-Correlation-Id";

    [Fact]
    public static async void GivenNewInstance_WhenCalledWithCorrelationIdHeader_ThenSetsCorrelationIdOnCorrelationIdService()
    {
        var correlationIdService = new CorrelationIdService();
        var expectedCorrelationId = Guid.NewGuid().ToString();
        var mockHttpContext = BuildMockHttpContext(expectedCorrelationId);

        var sut = new CorrelationIdMiddleware((context) => Task.CompletedTask);
        await sut.Invoke(mockHttpContext.Object, correlationIdService);

        Assert.Equal(expectedCorrelationId, correlationIdService.CorrelationId);
    }

    private static Mock<HttpContext> BuildMockHttpContext(string expectedCorrelationId)
    {
        var mockHttpRequest = SetUpMockHttpRequest(expectedCorrelationId);
        var mockHttpResponse = SetUpMockHttpResponse();
     
        return SetUpMockHttpContext(mockHttpRequest.Object, mockHttpResponse.Object);
    }

    private static Mock<HttpContext> SetUpMockHttpContext(HttpRequest mockHttpRequest, HttpResponse mockHttpResponse)
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest);
        mockHttpContext.Setup(m => m.Response).Returns(mockHttpResponse);
        
        return mockHttpContext;
    }

    private static Mock<HttpResponse> SetUpMockHttpResponse()
    {
        var mockHttpResponse = new Mock<HttpResponse>();
        mockHttpResponse.Setup(m => m.Headers).Returns(new HeaderDictionary());
        
        return mockHttpResponse;
    }

    private static Mock<HttpRequest> SetUpMockHttpRequest(string expectedCorrelationId)
    {
        var mockHttpRequest = new Mock<HttpRequest>();
        mockHttpRequest.Setup(m => m.Headers).Returns(new HeaderDictionary()
        {
            { _correlationIdHeader, expectedCorrelationId }
        });

        return mockHttpRequest;
    }
}

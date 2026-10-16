using System.Net;

public abstract class TavilyUnitTestBase
{
    protected static HttpClient CreateClient(
        string response,
        Func<HttpRequestMessage, Task> onRequest,
        HttpStatusCode statusCode = HttpStatusCode.OK
    ) => new(new StubHandler(response, onRequest, statusCode));

    protected sealed class StubHandler(
        string response,
        Func<HttpRequestMessage, Task>? onRequest = null,
        HttpStatusCode statusCode = HttpStatusCode.OK
    ) : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            if (onRequest is not null)
            {
                await onRequest(request);
            }

            return new HttpResponseMessage(statusCode) { Content = new StringContent(response) };
        }
    }
}

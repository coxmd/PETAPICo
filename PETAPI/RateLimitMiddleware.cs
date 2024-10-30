using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace PETAPI
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxRequestsPerSecond;
        private static readonly ConcurrentDictionary<string, int> _requestCounts = new ConcurrentDictionary<string, int>();

        public RateLimitMiddleware(RequestDelegate next, int maxRequestsPerSecond)
        {
            _next = next;
            _maxRequestsPerSecond = maxRequestsPerSecond;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress.ToString();
            var requestCount = _requestCounts.GetOrAdd(clientIp, 0);

            if (requestCount >= _maxRequestsPerSecond)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            _requestCounts[clientIp] = requestCount + 1;

            await _next(context);

            _requestCounts[clientIp] = requestCount;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kucoin.Api
{
    public interface IKucoinHttpClient : IDisposable
    {
        /// <summary>
        /// Get or set the timestamp provider.
        /// </summary>
        ITimestampProvider TimestampProvider { get; set; }

        /// <summary>
        /// Get or set the API request (default) rate limiter.
        /// </summary>
        IApiRateLimiter RateLimiter { get; set; }

        /// <summary>
        /// Get the options.
        /// 
        /// NOTE: The RequestRateLimit settings are applied at construction.
        ///       To change the request rate limiter settings at runtime,
        ///       use RateLimiter.Configure().
        /// </summary>
        KucoinApiOptions Options { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SignAsync(KucoinHttpRequest request, IKucoinApiUser user, CancellationToken token = default);

        /// <summary>
        /// Get request.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> GetAsync(string path, CancellationToken token = default);

        /// <summary>
        /// Get request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> GetAsync(KucoinHttpRequest request, CancellationToken token = default);

        /// <summary>
        /// Post request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> PostAsync(KucoinHttpRequest request, CancellationToken token = default);

        /// <summary>
        /// Put request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> PutAsync(KucoinHttpRequest request, CancellationToken token = default);

        /// <summary>
        /// Delete request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> DeleteAsync(KucoinHttpRequest request, CancellationToken token = default);
    }
}

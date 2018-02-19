﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kucoin.Api
{
    public interface IBinanceHttpClient : IDisposable
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
        BinanceApiOptions Options { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SignAsync(BinanceHttpRequest request, IBinanceApiUser user, CancellationToken token = default);

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
        Task<string> GetAsync(BinanceHttpRequest request, CancellationToken token = default);

        /// <summary>
        /// Post request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> PostAsync(BinanceHttpRequest request, CancellationToken token = default);

        /// <summary>
        /// Put request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> PutAsync(BinanceHttpRequest request, CancellationToken token = default);

        /// <summary>
        /// Delete request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> DeleteAsync(BinanceHttpRequest request, CancellationToken token = default);
    }
}

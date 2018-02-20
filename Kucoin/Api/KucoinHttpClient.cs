using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kucoin.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
//using Newtonsoft.Json.Linq;

namespace Kucoin.Api
{
    public sealed class KucoinHttpClient : IKucoinHttpClient
    {
        #region Public Constants

        /// <summary>
        /// Get the base endpoint URL.
        /// </summary>
        public static readonly string EndpointUrl = "https://api.kucoin.com";

        /// <summary>
        /// Get the successful test response string.
        /// </summary>
        public static readonly string SuccessfulTestResponse = "{}";

        #endregion Public Constants

        #region Public Properties

        /// <summary>
        /// Singleton.
        /// </summary>
        public static KucoinHttpClient Instance => Initializer.Value;

        public ITimestampProvider TimestampProvider { get; set; }

        public IApiRateLimiter RateLimiter { get; set; }

        public KucoinApiOptions Options { get; }

        #endregion Public Properties

        #region Internal

        /// <summary>
        /// Lazy initializer.
        /// </summary>
        internal static Lazy<KucoinHttpClient> Initializer
            = new Lazy<KucoinHttpClient>(() => new KucoinHttpClient(), true);

        #endregion Internal

        #region Private Fields

        private readonly HttpClient _httpClient;

        private readonly ILogger<KucoinHttpClient> _logger;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timestampProvider">The timestamp provider.</param>
        /// <param name="rateLimiter">The rate limiter (auto configured).</param>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        internal KucoinHttpClient(ITimestampProvider timestampProvider = null, IApiRateLimiter rateLimiter = null, IOptions<KucoinApiOptions> options = null, ILogger<KucoinHttpClient> logger = null)
        {
            TimestampProvider = timestampProvider ?? new TimestampProvider();
            RateLimiter = rateLimiter ?? new ApiRateLimiter();
            Options = options?.Value ?? new KucoinApiOptions();
            _logger = logger;

            try
            {
                // Configure request rate limiter.
                RateLimiter.Configure(TimeSpan.FromMinutes(Options.RequestRateLimit.DurationMinutes), Options.RequestRateLimit.Count);
                // Configure request burst rate limiter.
                RateLimiter.Configure(TimeSpan.FromSeconds(Options.RequestRateLimit.BurstDurationSeconds), Options.RequestRateLimit.BurstCount);
            }
            catch (Exception e)
            {
                var message = $"{nameof(KucoinHttpClient)}: Failed to configure request rate limiter.";
                _logger?.LogError(e, message);
                throw new KucoinApiException(message, e);
            }

            var uri = new Uri(EndpointUrl);

            try
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = uri,
                    Timeout = TimeSpan.FromSeconds(60)
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(KucoinHttpClient)}: Failed to create HttpClient.";
                _logger?.LogError(e, message);
                throw new KucoinApiException(message, e);
            }

            if (Options.ServicePointManagerConnectionLeaseTimeoutMilliseconds > 0)
            {
                try
                {
                    // FIX: Singleton HttpClient doesn't respect DNS changes.
                    // https://github.com/dotnet/corefx/issues/11224
                    var sp = ServicePointManager.FindServicePoint(uri);
                    sp.ConnectionLeaseTimeout = Options.ServicePointManagerConnectionLeaseTimeoutMilliseconds;
                }
                catch (Exception e)
                {
                    var message = $"{nameof(KucoinHttpClient)}: Failed to set {nameof(ServicePointManager)}.ConnectionLeaseTimeout.";
                    _logger?.LogError(e, message);
                    throw new KucoinApiException(message, e);
                }
            }

            try
            {
                var version = GetType().Assembly.GetName().Version;

                var versionString = $"{version.Major}.{version.Minor}.{version.Build}{(version.Revision > 0 ? $".{version.Revision}" : string.Empty)}";

                _httpClient.DefaultRequestHeaders.Add("User-Agent", $"Kucoin/{versionString} (.NET;)");
            }
            catch (Exception e)
            {
                var message = $"{nameof(KucoinHttpClient)}: Failed to set User-Agent.";
                _logger?.LogError(e, message);
                throw new KucoinApiException(message, e);
            }
        }

        #endregion Constructors

        #region Public Methods

        public async Task SignAsync(KucoinHttpRequest request, IKucoinApiUser user, CancellationToken token = default)
        {
            Throw.IfNull(request, nameof(request));
            Throw.IfNull(user, nameof(user));

            var timestamp = TimestampProvider != null
                ? await TimestampProvider.GetTimestampAsync(this, token).ConfigureAwait(false)
                : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            request.AddParameter("timestamp", timestamp);

            var signature = user.Sign(request.TotalParams);

            request.AddParameter("signature", signature);
        }

        public Task<string> GetAsync(string path, CancellationToken token = default)
            => GetAsync(new KucoinHttpRequest(path), token);

        public Task<string> GetAsync(KucoinHttpRequest request, CancellationToken token = default)
        {
            return RequestAsync(HttpMethod.Get, request, token);
        }

        public Task<string> PostAsync(KucoinHttpRequest request, CancellationToken token = default)
        {
            return RequestAsync(HttpMethod.Post, request, token);
        }

        public Task<string> PutAsync(KucoinHttpRequest request, CancellationToken token = default)
        {
            return RequestAsync(HttpMethod.Put, request, token);
        }

        public Task<string> DeleteAsync(KucoinHttpRequest request, CancellationToken token = default)
        {
            return RequestAsync(HttpMethod.Delete, request, token);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<string> RequestAsync(HttpMethod method, KucoinHttpRequest request, CancellationToken token = default)
        {
            Throw.IfNull(request, nameof(request));

            token.ThrowIfCancellationRequested();

            var requestMessage = request.CreateMessage(method);

            _logger?.LogDebug($"{nameof(KucoinHttpClient)}.{nameof(RequestAsync)}: [{method.Method}] \"{requestMessage.RequestUri}\"");

            using (var response = await _httpClient.SendAsync(requestMessage, token).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync()
                        .ConfigureAwait(false);

                    _logger?.LogDebug($"{nameof(KucoinHttpClient)}: \"{json}\"");

                    return json;
                }

                if (response.StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    throw new KucoinUnknownStatusException();
                }

                var error = await response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);

                var errorCode = 0;
                string errorMessage = null;

                // ReSharper disable once InvertIf
                if (!string.IsNullOrWhiteSpace(error) && error.IsJsonObject())
                {
                    try // to parse server error response.
                    {
                        var jObject = JObject.Parse(error);

                        errorCode = jObject["code"]?.Value<int>() ?? 0;
                        errorMessage = jObject["msg"]?.Value<string>();
                    }
                    catch (Exception e)
                    {
                        _logger?.LogError(e, $"{nameof(KucoinHttpClient)}.{nameof(RequestAsync)} failed to parse server error response: \"{error}\"");
                    }
                }

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (response.StatusCode)
                {
                    case (HttpStatusCode)429:
                        throw new KucoinRequestRateLimitExceededException(response.ReasonPhrase, errorCode, errorMessage);
                    case (HttpStatusCode)418:
                        throw new KucoinRequestRateLimitIpBanException(response.ReasonPhrase, errorCode, errorMessage);
                    default:
                        throw new KucoinHttpException(response.StatusCode, response.ReasonPhrase, errorCode, errorMessage);
                }
            }
        }

        #endregion Private Methods

        #region IDisposable

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _httpClient?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable
    }
}

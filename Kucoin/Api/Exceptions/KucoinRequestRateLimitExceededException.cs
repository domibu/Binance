using System.Net;

// ReSharper disable once CheckNamespace
namespace Kucoin.Api
{
    /// <summary>
    /// Kucoin request rate limit exceeded exception.
    /// 
    /// HTTP 429 return code is used when breaking a request rate limit.
    /// When a 429 is recieved, it's your obligation as an API to back off and not spam the API.
    /// </summary>
    public sealed class KucoinRequestRateLimitExceededException : KucoinHttpException
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public KucoinRequestRateLimitExceededException(string reasonPhrase, int errorCode, string errorMessage)
            : base((HttpStatusCode)429, reasonPhrase, errorCode, errorMessage)
        { }

        #endregion Constructors
    }
}

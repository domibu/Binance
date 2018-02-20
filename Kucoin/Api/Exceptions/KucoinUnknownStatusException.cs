using System.Net;

// ReSharper disable once CheckNamespace
namespace Kucoin.Api
{
    /// <summary>
    /// Kucoin unknown status exception.
    /// 
    /// Thrown when the API successfully sent a request but not get a response
    /// within the timeout period (HTTP 504 return code). It is important to
    /// NOT treat this as a failure; the execution status is UNKNOWN and
    /// could have been a success.
    /// </summary>
    public sealed class KucoinUnknownStatusException : KucoinHttpException
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public KucoinUnknownStatusException()
            : base(HttpStatusCode.GatewayTimeout, null, 0, null)
        { }

        #endregion Constructors
    }
}

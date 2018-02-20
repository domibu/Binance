using System;

// ReSharper disable once CheckNamespace
namespace Kucoin.Api
{
    /// <summary>
    /// Kucoin API exception.
    /// </summary>
    public class KucoinApiException : Exception
    {
        #region Constructors

        public KucoinApiException()
        { }

        public KucoinApiException(string message)
            : base(message)
        { }

        public KucoinApiException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion Constructors
    }
}

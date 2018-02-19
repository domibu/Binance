using System.Collections.Generic;
using Kucoin.Market;
using Kucoin.Utility;

namespace Kucoin.Cache.Events
{
    public sealed class CandlestickCacheEventArgs : CacheEventArgs
    {
        #region Public Properties

        /// <summary>
        /// The candlesticks.
        /// </summary>
        public IEnumerable<Candlestick> Candlesticks { get; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="candlesticks">The candlesticks.</param>
        public CandlestickCacheEventArgs(IEnumerable<Candlestick> candlesticks)
        {
            Throw.IfNull(candlesticks, nameof(candlesticks));

            Candlesticks = candlesticks;
        }

        #endregion Constructors
    }
}

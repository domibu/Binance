using System.Collections.Generic;
using Kucoin.Market;
using Kucoin.Utility;

namespace Kucoin.Cache.Events
{
    public sealed class TradeCacheEventArgs : CacheEventArgs
    {
        #region Public Properties

        /// <summary>
        /// The latest trades.
        /// </summary>
        public IEnumerable<Trade> Trades { get; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="trades">The latest trades.</param>
        public TradeCacheEventArgs(IEnumerable<Trade> trades)
        {
            Throw.IfNull(trades, nameof(trades));

            Trades = trades;
        }

        #endregion Constructors
    }
}

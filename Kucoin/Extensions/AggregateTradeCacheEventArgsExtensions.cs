using System.Linq;
using Kucoin.Market;

// ReSharper disable once CheckNamespace
namespace Kucoin.Cache.Events
{
    public static class AggregateTradeCacheEventArgsExtensions
    {
        /// <summary>
        /// Get latest trade.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static AggregateTrade LatestTrade(this AggregateTradeCacheEventArgs args)
        {
            return args.Trades.LastOrDefault();
        }
    }
}

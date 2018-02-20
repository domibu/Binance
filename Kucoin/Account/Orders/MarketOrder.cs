using Kucoin.Api;

namespace Kucoin.Account.Orders
{
    public class MarketOrder : ClientOrder
    {
        #region Public Properties

        /// <summary>
        /// Get the order type.
        /// </summary>
        public override OrderType Type => OrderType.Market;

        #endregion Public Properties

        #region Constructors

        public MarketOrder(IKucoinApiUser user)
            : base(user)
        { }

        #endregion Constructors
    }
}

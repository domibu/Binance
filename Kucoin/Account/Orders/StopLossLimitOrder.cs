
using Kucoin.Api;

namespace Kucoin.Account.Orders
{
    public sealed class StopLossLimitOrder : StopLimitOrder
    {
        #region Public Properties

        public override OrderType Type => OrderType.StopLossLimit;

        #endregion Public Properties

        #region Constructors

        public StopLossLimitOrder(IKucoinApiUser user)
            : base(user)
        { }

        #endregion Constructors
    }
}

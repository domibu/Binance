
using Kucoin.Api;

namespace Kucoin.Account.Orders
{
    public sealed class StopLossOrder : StopOrder
    {
        #region Public Properties

        public override OrderType Type => OrderType.StopLoss;

        #endregion Public Properties

        #region Constructors

        public StopLossOrder(IKucoinApiUser user)
            : base(user)
        { }

        #endregion Constructors
    }
}

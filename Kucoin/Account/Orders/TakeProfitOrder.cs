using Kucoin.Api;

namespace Kucoin.Account.Orders
{
    public sealed class TakeProfitOrder : StopOrder
    {
        #region Public Properties

        public override OrderType Type => OrderType.TakeProfit;

        #endregion Public Properties

        #region Constructors

        public TakeProfitOrder(IKucoinApiUser user)
            : base(user)
        { }

        #endregion Constructors
    }
}

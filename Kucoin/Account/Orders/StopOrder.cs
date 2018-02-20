
using Kucoin.Api;

namespace Kucoin.Account.Orders
{
    public abstract class StopOrder : MarketOrder, IStopOrder
    {
        #region Public Properties

        /// <summary>
        /// Get or set the stop price.
        /// </summary>
        public decimal StopPrice { get; set; }

        #endregion Public Properties

        #region Constructors

        protected StopOrder(IKucoinApiUser user)
            : base(user)
        { }

        #endregion Constructors
    }
}

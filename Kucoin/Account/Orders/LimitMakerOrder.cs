using Kucoin.Api;
using System;

namespace Kucoin.Account.Orders
{
    public sealed class LimitMakerOrder : LimitOrder
    {
        #region Public Properties

        public override OrderType Type => OrderType.LimitMaker;

        public override TimeInForce TimeInForce
        {
            get => TimeInForce.GTC;
            set
            {
                if (value != TimeInForce.GTC)
                    throw new ArgumentException($"{nameof(TimeInForce)} must be {TimeInForce.GTC}.");
            }
        }

        #endregion Public Properties

        #region Constructors

        public LimitMakerOrder(IKucoinApiUser user)
            : base(user)
        { }

        #endregion Constructors
    }
}

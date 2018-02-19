// ReSharper disable InconsistentNaming
namespace Kucoin.Account.Orders
{
    public enum TimeInForce
    {
        /// <summary>
        /// Good 'til canceled.
        /// </summary>
        GTC,

        /// <summary>
        /// Immediate or cancel.
        /// </summary>
        IOC,

        /// <summary>
        /// Fill or kill.
        /// </summary>
        FOK
    }
}

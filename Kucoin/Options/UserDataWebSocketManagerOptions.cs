
// ReSharper disable once CheckNamespace
using Kucoin.WebSocket.UserData;

namespace Kucoin
{
    public sealed class UserDataWebSocketManagerOptions
    {
        /// <summary>
        /// Keep-alive timer period.
        /// </summary>
        public int KeepAliveTimerPeriod { get; set; } = UserDataKeepAliveTimer.PeriodDefault;
    }
}

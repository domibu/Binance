using System;
using System.Threading;
using System.Threading.Tasks;
using Kucoin.Api;
using Kucoin.WebSocket.Events;

namespace Kucoin.WebSocket.UserData
{
    public interface IUserDataWebSocketManager
    {
        /// <summary>
        /// The account update event.
        /// </summary>
        event EventHandler<AccountUpdateEventArgs> AccountUpdate;

        /// <summary>
        /// The order update event.
        /// </summary>
        event EventHandler<OrderUpdateEventArgs> OrderUpdate;

        /// <summary>
        /// The trade update event.
        /// </summary>
        event EventHandler<AccountTradeUpdateEventArgs> TradeUpdate;

        /// <summary>
        /// Get the web socket client.
        /// </summary>
        ISingleUserDataWebSocketClient Client { get; }

        /// <summary>
        /// Subscribe to the specified user and begin streaming.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="callback"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SubscribeAndStreamAsync(IKucoinApiUser user, Action<UserDataEventArgs> callback, CancellationToken token = default);
    }
}

﻿using System;
using Kucoin.WebSocket.Events;

namespace Kucoin.WebSocket
{
    public interface IAggregateTradeWebSocketClient : IKucoinWebSocketClient
    {
        /// <summary>
        /// The aggregate trade event. Receive aggregate trade events for all
        /// subscribed symbols.
        /// </summary>
        event EventHandler<AggregateTradeEventArgs> AggregateTrade;

        /// <summary>
        /// Subscribe to the specified symbol (for use with combined streams).
        /// Call <see cref="IWebSocketStream"/> StreamAsync to begin streaming.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe.</param>
        /// <param name="callback">An event callback (optional).</param>
        void Subscribe(string symbol, Action<AggregateTradeEventArgs> callback);

        /// <summary>
        /// Unsubscribe a callback from symbol events. If no callback is
        /// specified, then unsubscribe symbol (all callbacks).
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="callback"></param>
        void Unsubscribe(string symbol, Action<AggregateTradeEventArgs> callback);
    }
}

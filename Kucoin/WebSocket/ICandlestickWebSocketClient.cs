using System;
using Kucoin.Market;
using Kucoin.WebSocket.Events;

namespace Kucoin.WebSocket
{
    public interface ICandlestickWebSocketClient : IBinanceWebSocketClient
    {
        /// <summary>
        /// The candlestick event.
        /// </summary>
        event EventHandler<CandlestickEventArgs> Candlestick;

        /// <summary>
        /// Subscribe to the specified symbol (for use with combined streams).
        /// Call <see cref="IWebSocketStream"/> StreamAsync to begin streaming.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="callback">An event callback.</param>
        void Subscribe(string symbol, CandlestickInterval interval, Action<CandlestickEventArgs> callback);

        /// <summary>
        /// Unsubscribe a callback from symbol events. If no callback is
        /// specified, then unsubscribe symbol & interval (all callbacks).
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval">The interval.</param>
        /// <param name="callback"></param>
        void Unsubscribe(string symbol, CandlestickInterval interval, Action<CandlestickEventArgs> callback);
    }
}

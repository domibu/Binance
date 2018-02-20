using System;

namespace Kucoin.WebSocket
{
    public interface IKucoinWebSocketClient
    {
        /// <summary>
        /// The web socket client open event.
        /// </summary>
        event EventHandler<EventArgs> Open;

        /// <summary>
        /// The web socket client close event.
        /// </summary>
        event EventHandler<EventArgs> Close;

        /// <summary>
        /// The web socket stream.
        /// </summary>
        IWebSocketStream WebSocket { get; }

        /// <summary>
        /// Unsubscribe all streams.
        /// </summary>
        void UnsubscribeAll();
    }
}

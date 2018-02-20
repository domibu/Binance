using Kucoin.Utility;
using System;

namespace Kucoin.WebSocket.Manager
{
    public class WebSocketManagerException : Exception
    {
        public IKucoinWebSocketClient Client { get; }

        public WebSocketManagerException(IKucoinWebSocketClient client)
            : this(client, null, null)
        { }

        public WebSocketManagerException(IKucoinWebSocketClient client, string message)
            : this(client, message, null)
        { }

        public WebSocketManagerException(IKucoinWebSocketClient client, Exception innerException)
            : this(client, null, innerException)
        { }

        public WebSocketManagerException(IKucoinWebSocketClient client, string message, Exception innerException)
            : base(message, innerException)
        {
            Throw.IfNull(client, nameof(client));

            Client = client;
        }
    }
}

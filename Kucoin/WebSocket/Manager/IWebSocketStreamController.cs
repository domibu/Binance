using Kucoin.Utility;

namespace Kucoin.WebSocket.Manager
{
    public interface IWebSocketStreamController : ITaskController
    {
        /// <summary>
        /// Get the web socket stream.
        /// </summary>
        IWebSocketStream WebSocket { get; }
    }
}

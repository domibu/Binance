using System.Threading.Tasks;

namespace Kucoin.WebSocket.Manager
{
    public interface IBinanceWebSocketClientAdapter
    {
        Task Task { get; }
    }
}

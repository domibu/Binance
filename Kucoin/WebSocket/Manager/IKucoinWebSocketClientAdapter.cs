﻿using System.Threading.Tasks;

namespace Kucoin.WebSocket.Manager
{
    public interface IKucoinWebSocketClientAdapter
    {
        Task Task { get; }
    }
}

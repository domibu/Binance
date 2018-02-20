using System;
using Kucoin.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kucoin.WebSocket.UserData
{
    public sealed class WebSocketStreamProvider : IWebSocketStreamProvider
    {
        private readonly IServiceProvider _services;

        public WebSocketStreamProvider()
        { }

        public WebSocketStreamProvider(IServiceProvider services)
        {
            Throw.IfNull(services, nameof(services));

            _services = services;
        }

        public IWebSocketStream CreateStream()
        {
            if (_services == null)
                return new KucoinWebSocketStream();

            var client = _services.GetService<IWebSocketClient>();
            var loggerFactory = _services.GetService<ILoggerFactory>();

            return new KucoinWebSocketStream(client, loggerFactory.CreateLogger<KucoinWebSocketStream>());
        }
    }
}

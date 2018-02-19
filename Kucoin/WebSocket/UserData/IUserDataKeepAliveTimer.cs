using System;
using System.Collections.Generic;
using Kucoin.Api;

namespace Kucoin.WebSocket.UserData
{
    public interface IUserDataKeepAliveTimer : IDisposable
    {
        TimeSpan Period { get; set; }

        IEnumerable<IBinanceApiUser> Users { get; }

        void Add(IBinanceApiUser user, string listenKey);

        void Remove(IBinanceApiUser user);

        void RemoveAll();
    }
}

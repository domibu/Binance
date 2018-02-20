using System;
using System.Collections.Generic;
using Kucoin.Api;

namespace Kucoin.WebSocket.UserData
{
    public interface IUserDataKeepAliveTimer : IDisposable
    {
        TimeSpan Period { get; set; }

        IEnumerable<IKucoinApiUser> Users { get; }

        void Add(IKucoinApiUser user, string listenKey);

        void Remove(IKucoinApiUser user);

        void RemoveAll();
    }
}

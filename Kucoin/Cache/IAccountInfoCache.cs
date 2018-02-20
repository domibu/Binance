using System;
using System.Threading;
using System.Threading.Tasks;
using Kucoin.Account;
using Kucoin.Api;
using Kucoin.Cache.Events;
using Kucoin.WebSocket.UserData;

namespace Kucoin.Cache
{
    public interface IAccountInfoCache
    {
        #region Events

        /// <summary>
        /// AccountInfo cache update event.
        /// </summary>
        event EventHandler<AccountInfoCacheEventArgs> Update;

        #endregion Events

        #region Properties

        /// <summary>
        /// The account information. Can be null if not yet synchronized or out-of-sync.
        /// </summary>
        AccountInfo AccountInfo { get; }

        /// <summary>
        /// The manager that provides account info synchronization.
        /// </summary>
        IUserDataWebSocketManager Client { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="callback"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SubscribeAndStreamAsync(IKucoinApiUser user, Action<AccountInfoCacheEventArgs> callback, CancellationToken token = default);

        /// <summary>
        /// Link to a subscribed <see cref="IUserDataWebSocketManager"/>.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        void LinkTo(IUserDataWebSocketManager manager, Action<AccountInfoCacheEventArgs> callback = null);

        /// <summary>
        /// Unlink from client.
        /// </summary>
        void UnLink();

        #endregion Methods
    }
}

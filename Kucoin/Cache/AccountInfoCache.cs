﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Kucoin.Account;
using Kucoin.Api;
using Kucoin.Cache.Events;
using Kucoin.Utility;
using Kucoin.WebSocket.Events;
using Kucoin.WebSocket.UserData;
using Microsoft.Extensions.Logging;

namespace Kucoin.Cache
{
    public sealed class AccountInfoCache : WebSocketClientCache<IUserDataWebSocketManager, UserDataEventArgs, AccountInfoCacheEventArgs>, IAccountInfoCache
    {
        #region Public Properties

        public AccountInfo AccountInfo { get; private set; }

        #endregion Public Properties

        #region Constructors

        public AccountInfoCache(IKucoinApi api, IUserDataWebSocketManager manager, ILogger<AccountInfoCache> logger = null)
            : base(api, manager, logger)
        { }

        #endregion Constructors

        #region Public Methods

        public Task SubscribeAndStreamAsync(IKucoinApiUser user, Action<AccountInfoCacheEventArgs> callback, CancellationToken token = default)
        {
            Throw.IfNull(user, nameof(user));

            base.LinkTo(Client, callback);

            return Client.SubscribeAndStreamAsync(user, ClientCallback, token);
        }

        public override void LinkTo(IUserDataWebSocketManager manager, Action<AccountInfoCacheEventArgs> callback = null)
        {
            // Confirm client is subscribed to only one stream.
            if (manager.Client.WebSocket.IsCombined)
                throw new InvalidOperationException($"{nameof(AccountInfoCache)} can only link to {nameof(IUserDataWebSocketClient)} events from a single stream (not combined streams).");

            base.LinkTo(manager, callback);
            Client.AccountUpdate += OnClientEvent;
        }

        public override void UnLink()
        {
            Client.AccountUpdate -= OnClientEvent;
            base.UnLink();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override ValueTask<AccountInfoCacheEventArgs> OnAction(UserDataEventArgs @event)
        {
            if (!(@event is AccountUpdateEventArgs accountInfoEvent))
                return new ValueTask<AccountInfoCacheEventArgs>();

            AccountInfo = accountInfoEvent.AccountInfo;

            return new ValueTask<AccountInfoCacheEventArgs>(new AccountInfoCacheEventArgs(AccountInfo));
        }

        #endregion Protected Methods
    }
}

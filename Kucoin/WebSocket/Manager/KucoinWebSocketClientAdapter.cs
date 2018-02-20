using System;
using System.Threading;
using System.Threading.Tasks;
using Kucoin.Utility;
using Microsoft.Extensions.Logging;

namespace Kucoin.WebSocket.Manager
{
    internal abstract class KucoinWebSocketClientAdapter<TWebSocketClient> : IKucoinWebSocketClient, IKucoinWebSocketClientAdapter
        where TWebSocketClient : IKucoinWebSocketClient
    {
        #region Public Events

        public event EventHandler<EventArgs> Open
        {
            add => Client.Open += value;
            remove => Client.Open -= value;
        }

        public event EventHandler<EventArgs> Close
        {
            add => Client.Close += value;
            remove => Client.Close -= value;
        }

        #endregion Public Events

        #region Public Properties

        public IWebSocketStream WebSocket => Client.WebSocket;

        public Task Task { get; protected set; } = Task.CompletedTask;

        protected readonly object Sync = new object();

        #endregion Public Properties

        #region Protected Properties & Fields

        protected ITaskController Controller => Manager.GetController(Client);

        protected readonly IKucoinWebSocketManager Manager;

        protected readonly TWebSocketClient Client;

        protected readonly ILogger<IKucoinWebSocketManager> Logger;

        protected readonly Action<Exception> OnError;

        #endregion Protected Properties & Fields

        #region Constructors

        protected KucoinWebSocketClientAdapter(IKucoinWebSocketManager manager, TWebSocketClient client, ILogger<IKucoinWebSocketManager> logger = null, Action<Exception> onError = null)
        {
            Throw.IfNull(manager, nameof(manager));
            Throw.IfNull(client, nameof(client));

            Manager = manager;
            Client = client;
            Logger = logger;
            OnError = onError;
        }

        #endregion Constructors

        #region Public Methods

        public void UnsubscribeAll()
        {
            lock (Sync)
            {
                Task = Task.ContinueWith(async _ =>
                {
                    try
                    {
                        Logger?.LogDebug($"{GetType().Name}.{nameof(UnsubscribeAll)}: Cancel streaming...  [thread: {Thread.CurrentThread.ManagedThreadId}]");
                        await Controller.CancelAsync()
                            .ConfigureAwait(false);

                        Client.UnsubscribeAll();

                        if (!Manager.IsAutoStreamingDisabled && !Controller.IsActive)
                        {
                            Logger?.LogDebug($"{GetType().Name}.{nameof(UnsubscribeAll)}: Begin streaming...  [thread: {Thread.CurrentThread.ManagedThreadId}]");
                            Controller.Begin();
                        }
                    }
                    catch (OperationCanceledException) { /* ignored */ }
                    catch (Exception e)
                    {
                        Logger?.LogError(e, $"{GetType().Name}.{nameof(UnsubscribeAll)}: Failed.  [thread: {Thread.CurrentThread.ManagedThreadId}]");
                        OnError?.Invoke(e);
                    }
                });
            }
        }

        #endregion Public Methods
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Kucoin.Utility;

// ReSharper disable once CheckNamespace
namespace Kucoin.WebSocket.Manager
{
    public static class KucoinWebSocketManagerExtensions
    {
        /// <summary>
        /// Get the <see cref="IRetryTaskController"/> associated with the
        /// <see cref="IKucoinWebSocketClient"/> web socket stream.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static IRetryTaskController GetController(this IKucoinWebSocketManager manager, IKucoinWebSocketClient client)
        {
            Throw.IfNull(manager, nameof(manager));
            Throw.IfNull(client, nameof(client));

            return manager.GetController(client.WebSocket);
        }

        /// <summary>
        /// Get all managed <see cref="IKucoinWebSocketClient"/> clients.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static IEnumerable<IKucoinWebSocketClient> Clients(this IKucoinWebSocketManager manager)
        {
            Throw.IfNull(manager, nameof(manager));

            yield return manager.AggregateTradeClient;
            yield return manager.CandlestickClient;
            yield return manager.DepthClient;
            yield return manager.StatisticsClient;
            yield return manager.TradeClient;
        }

        /// <summary>
        /// Begin all controller actions.
        /// </summary>
        /// <param name="manager"></param>
        public static void BeginAll(this IKucoinWebSocketManager manager)
        {
            Throw.IfNull(manager, nameof(manager));

            foreach (var client in Clients(manager))
            {
                GetController(manager, client).Begin();
            }
        }

        /// <summary>
        /// Cancel all controller actions.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static async Task CancelAllAsync(this IKucoinWebSocketManager manager)
        {
            Throw.IfNull(manager, nameof(manager));

            foreach (var client in Clients(manager))
            {
                await GetController(manager, client)
                    .CancelAsync()
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Cancel all controller actions and unsubscribe all streams from all clients.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static async Task UnsubscribeAllAsync(this IKucoinWebSocketManager manager)
        {
            Throw.IfNull(manager, nameof(manager));

            await CancelAllAsync(manager)
                .ConfigureAwait(false);

            var wasAutoStreamingDisabled = manager.IsAutoStreamingDisabled;
            manager.IsAutoStreamingDisabled = true; // disable auto-streaming.

            foreach (var client in Clients(manager))
            {
                client.UnsubscribeAll();

                // Wait for adapter asynchronous operation to complete.
                await ((IKucoinWebSocketClientAdapter)client).Task
                    .ConfigureAwait(false);
            }

            manager.IsAutoStreamingDisabled = wasAutoStreamingDisabled; // restore.
        }
    }
}

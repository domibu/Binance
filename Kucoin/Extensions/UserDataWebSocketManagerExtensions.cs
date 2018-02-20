using System.Threading;
using System.Threading.Tasks;
using Kucoin.Api;

// ReSharper disable once CheckNamespace
namespace Kucoin.WebSocket.UserData
{
    public static class UserDataWebSocketManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SubscribeAndStreamAsync(this IUserDataWebSocketManager manager, IKucoinApiUser user, CancellationToken token)
            => manager.SubscribeAndStreamAsync(user, null, token);
    }
}

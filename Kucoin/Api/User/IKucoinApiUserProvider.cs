// ReSharper disable once CheckNamespace
namespace Kucoin.Api
{
    public interface IKucoinApiUserProvider
    {
        /// <summary>
        /// Create an API user.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="apiSecret">The API secret (optional)</param>
        /// <returns></returns>
        IKucoinApiUser CreateUser(string apiKey, string apiSecret = null);
    }
}

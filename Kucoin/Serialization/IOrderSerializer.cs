using Kucoin.Account.Orders;
using Kucoin.Api;
using System.Collections.Generic;

namespace Kucoin.Serialization
{
    public interface IOrderSerializer
    {
        /// <summary>
        /// Deserialize JSON to an <see cref="Order"/>.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Order Deserialize(string json, Order order);

        /// <summary>
        /// Deserialize JSON to an <see cref="Order"/>.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Order Deserialize(string json, IKucoinApiUser user);

        /// <summary>
        /// Deserialize JSON to multiple <see cref="Order"/> instances.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IEnumerable<Order> DeserializeMany(string json, IKucoinApiUser user);

        /// <summary>
        /// Serialize an <see cref="Order"/> to JSON.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        string Serialize(Order order);
    }
}

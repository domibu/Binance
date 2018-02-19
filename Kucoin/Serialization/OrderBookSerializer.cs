using Kucoin.Market;
using Kucoin.Utility;
using Newtonsoft.Json;

namespace Kucoin.Serialization
{
    public class OrderBookSerializer : IOrderBookSerializer
    {
        public OrderBookJsonConverter JsonConverter { get; }

        private readonly JsonSerializerSettings _settings;

        public OrderBookSerializer()
        {
            JsonConverter = new OrderBookJsonConverter();

            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(JsonConverter);
        }

        public virtual OrderBook Deserialize(string json)
        {
            Throw.IfNullOrWhiteSpace(json, nameof(json));

            return JsonConvert.DeserializeObject<OrderBook>(json, _settings);
        }

        public virtual string Serialize(OrderBook orderBook)
        {
            Throw.IfNull(orderBook, nameof(orderBook));

            return JsonConvert.SerializeObject(orderBook, _settings);
        }
    }
}

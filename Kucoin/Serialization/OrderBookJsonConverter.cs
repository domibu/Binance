using System;
using System.Linq;
using Kucoin.Market;
using Kucoin.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kucoin.Serialization
{
    public sealed class OrderBookJsonConverter : JsonConverter
    {
        /// <summary>
        /// Get or set flag to include OrderBook.Symbol in JSON.
        /// </summary>
        public bool SerializeSymbol { get; set; } = true;

        private const string KeySymbol = "symbol";
        private const string KeyLastUpdateId = "lastUpdateId";
        private const string KeyData = "data";
        private const string KeyBids = "BUY";
        private const string KeyAsks = "SELL";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderBook);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var symbol = jObject[KeySymbol].Value<string>();

            long lastUpdateId = 1;//jObject[KeyLastUpdateId].Value<long>();

            var data = jObject[KeyData];

            //.Select(_ => (_[0].Value<decimal>(), _[1].Value<decimal>()))
            //.ToArray();

            var bids = data[KeyBids]
                .Select(_ => (_[0].Value<decimal>(), _[1].Value<decimal>()))
                .ToArray();

            var asks = data[KeyAsks]
                .Select(_ => (_[0].Value<decimal>(), _[1].Value<decimal>()))
                .ToArray();

            return new OrderBook(symbol, lastUpdateId, bids, asks);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is OrderBook orderBook))
                return;

            var jObject = new JObject();

            if (SerializeSymbol)
            {
                jObject.Add(new JProperty(KeySymbol, orderBook.Symbol));
            }

            jObject.Add(new JProperty(KeyLastUpdateId, orderBook.LastUpdateId));

            jObject.Add(new JProperty(KeyBids, orderBook.Bids.Select(_ => new JArray { _.Price, _.Quantity })));

            jObject.Add(new JProperty(KeyAsks, orderBook.Asks.Select(_ => new JArray { _.Price, _.Quantity })));

            jObject.WriteTo(writer);
        }

        /// <summary>
        /// Insert symbol into JSON.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string InsertSymbol(string json, string symbol)
        {
            Throw.IfNullOrWhiteSpace(json, nameof(json));
            Throw.IfNullOrWhiteSpace(symbol, nameof(symbol));

            return json.Insert(1, $"\"symbol\":\"{symbol.FormatSymbol()}\",");
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Binance;
using Binance.Market;
using Binance.WebSocket;
using Binance.WebSocket.Events;
using Binance.WebSocket.Manager;
using BinanceConsoleApp.Database;
using SQLite;
using Newtonsoft.Json;
//using SQLite.Net.Async;

namespace BinanceConsoleApp.Controllers
{
    internal class LiveOrderBook : IHandleCommand
    {
        object _threadLock = new object();
        static string _pathToDatabase = "OrderBook01.sqlite";
        string _kucoinOrderBookSymbol;
        int _kucoinOrderBookPeriod = 1000;
        Timer _kucoinOrderBookTimer;

        public async Task<bool> HandleAsync(string command, CancellationToken token = default)
        {
            if (!command.StartsWith("live ", StringComparison.OrdinalIgnoreCase) &&
                !command.Equals("live", StringComparison.OrdinalIgnoreCase))
                return false;

            var args = command.Split(' ');

            var endpoint = "depth";
            if (args.Length > 1)
            {
                endpoint = args[1];
            }

            if (!endpoint.Equals("depth", StringComparison.OrdinalIgnoreCase)
                && !endpoint.Equals("book", StringComparison.OrdinalIgnoreCase))
                return false;

            string symbol = Symbol.BTC_USDT;
            if (args.Length > 2)
            {
                symbol = args[2];
                if (!Symbol.IsValid(symbol))
                {
                    lock (Program.ConsoleSync)
                    {
                        Console.WriteLine($"  Invalid symbol: \"{symbol}\"");
                    }
                    return true;
                }
            }

            var enable = true;
            if (args.Length > 3)
            {
                if (args[3].Equals("off", StringComparison.OrdinalIgnoreCase))
                    enable = false;
            }

            if (enable)
            {
                Program.ClientManager.DepthClient.Subscribe(symbol, 5, evt => OnDepthUpdate(evt));

                using (var conn = new SQLiteConnection(_pathToDatabase))
                {
                    conn.CreateTable<OrderBookSqlite>();
                }

                // Optionally, wait for asynchronous client adapter operation to complete.
                await ((IBinanceWebSocketClientAdapter)Program.ClientManager.DepthClient).Task;

                lock (Program.ConsoleSync)
                {
                    Console.WriteLine();
                    Console.WriteLine($"  ...live order book (depth) ENABLED for symbol: {symbol}");
                    Console.WriteLine();
                }

                _kucoinOrderBookTimer = new Timer(OrderBookTimerCallback, null, _kucoinOrderBookPeriod, Timeout.Infinite);
                _kucoinOrderBookSymbol = symbol;
            }
            else
            {
                Program.ClientManager.DepthClient.Unsubscribe(symbol, 5);

                // Optionally, wait for asynchronous client adapter operation to complete.
                await ((IBinanceWebSocketClientAdapter)Program.ClientManager.DepthClient).Task;

                lock (Program.ConsoleSync)
                {
                    Console.WriteLine();
                    Console.WriteLine($"  ...live order book (depth) DISABLED for symbol: {symbol}");
                    Console.WriteLine();
                }

                _kucoinOrderBookTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            return true;
        }

        private async void OrderBookTimerCallback(object state)
        {
            //lock (_threadLock)
            {
                try
                {
                    var orderBook = await Program.KucoinApi.GetOrderBookAsync(_kucoinOrderBookSymbol);

                    var top = OrderBookTop.Create(orderBook.Symbol, (orderBook.Bids.First().Price, orderBook.Bids.First().Quantity), (orderBook.Asks.First().Price, orderBook.Asks.First().Quantity));

                    using (var conn = new SQLiteConnection(_pathToDatabase, false))
                    {
                        conn.Insert(new OrderBookSqlite
                        {
                            Timestamp = DateTime.UtcNow,
                            Market = "Kucoin",
                            Symbol = _kucoinOrderBookSymbol,
                            DataType = 82,
                            Data = JsonConvert.SerializeObject(orderBook),
                            Bid = (double)top.Bid.Price,
                            Ask = (double)top.Ask.Price
                        });
                    }

                    lock (Program.ConsoleSync)
                    {
                        Console.WriteLine($"  Kucoin  {top.Symbol}  -  Bid: {top.Bid.Price:.00000000}  |  {top.MidMarketPrice():.00000000}  |  Ask: {top.Ask.Price:.00000000}  -  Spread: {top.Spread():.00000000}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    _kucoinOrderBookTimer.Change(_kucoinOrderBookPeriod, Timeout.Infinite);
                }
            }
        }

        private static void OnDepthUpdate(DepthUpdateEventArgs e)
        {
            var top = OrderBookTop.Create(e.Symbol, e.Bids.First(), e.Asks.First());

            using (var conn = new SQLiteConnection(_pathToDatabase, false))
            {
                conn.Insert(new OrderBookSqlite
                {
                    Timestamp = e.Time,
                    Market = "Binance",
                    Symbol = e.Symbol,
                    DataType = 81,
                    Data = JsonConvert.SerializeObject(e),
                    Bid = (double)top.Bid.Price,
                    Ask = (double)top.Ask.Price
                });
            }

            lock (Program.ConsoleSync)
            {
                Console.WriteLine($"  Binance {top.Symbol}  -  Bid: {top.Bid.Price:.00000000}  |  {top.MidMarketPrice():.00000000}  |  Ask: {top.Ask.Price:.00000000}  -  Spread: {top.Spread():.00000000}");
            }
        }

        public class TestPath
        {
            public static string GetTempFileName()
            {
#if NETFX_CORE
            var name = Guid.NewGuid () + ".sqlite";
            return Path.Combine (Windows.Storage.ApplicationData.Current.LocalFolder.Path, name);
#else
                return Path.GetTempFileName();
#endif
            }
        }
        }
    }

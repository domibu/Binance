using System;
using Kucoin.Api;
using Kucoin.Cache;
using Kucoin.Serialization;
using Kucoin.WebSocket;
using Kucoin.WebSocket.Manager;
using Kucoin.WebSocket.UserData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Kucoin
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKucoin(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IKucoinApiUserProvider, KucoinApiUserProvider>();
            services.AddSingleton<ITimestampProvider, TimestampProvider>();
            services.AddSingleton<IKucoinHttpClient>(s =>
            {
                if (!KucoinHttpClient.Initializer.IsValueCreated)
                {
                    // Replace initializer.
                    KucoinHttpClient.Initializer = new Lazy<KucoinHttpClient>(() =>
                        new KucoinHttpClient(
                            s.GetService<ITimestampProvider>(),
                            s.GetService<IApiRateLimiter>(),
                            s.GetService<IOptions<KucoinApiOptions>>(),
                            s.GetService<ILogger<KucoinHttpClient>>()), true);
                }

                return KucoinHttpClient.Instance;
            });
            services.AddTransient<IApiRateLimiter, ApiRateLimiter>();
            services.AddTransient<IRateLimiter, RateLimiter>();
            services.AddSingleton<IKucoinApi, KucoinApi>();

            // Cache
            services.AddTransient<ITradeCache, TradeCache>();
            services.AddTransient<IOrderBookCache, OrderBookCache>();
            services.AddTransient<IAccountInfoCache, AccountInfoCache>();
            services.AddTransient<ICandlestickCache, CandlestickCache>();
            services.AddTransient<IAggregateTradeCache, AggregateTradeCache>();
            services.AddTransient<ISymbolStatisticsCache, SymbolStatisticsCache>();

            // WebSocket
            services.AddTransient<IWebSocketClient, DefaultWebSocketClient>();
            services.AddTransient<IWebSocketStream, KucoinWebSocketStream>();
            services.AddSingleton<IWebSocketStreamProvider, WebSocketStreamProvider>();
            services.AddTransient<ITradeWebSocketClient, TradeWebSocketClient>();
            services.AddTransient<IDepthWebSocketClient, DepthWebSocketClient>();
            services.AddTransient<ICandlestickWebSocketClient, CandlestickWebSocketClient>();
            //services.AddTransient<IAggregateTradeWebSocketClient, AggregateTradeWebSocketClient>();
            services.AddTransient<ISymbolStatisticsWebSocketClient, SymbolStatisticsWebSocketClient>();
            services.AddTransient<IUserDataWebSocketClient, UserDataWebSocketClient>();
            services.AddTransient<ISingleUserDataWebSocketClient, SingleUserDataWebSocketClient>();
            services.AddTransient<IUserDataKeepAliveTimer, UserDataKeepAliveTimer>();
            services.AddTransient<IUserDataKeepAliveTimerProvider, UserDataKeepAliveTimerProvider>();
            services.AddTransient<IUserDataWebSocketManager, UserDataWebSocketManager>();
            services.AddTransient<IKucoinWebSocketManager, KucoinWebSocketManager>();

            // Serialization
            services.AddSingleton<IOrderBookTopSerializer, OrderBookTopSerializer>();
            services.AddSingleton<IOrderBookSerializer, OrderBookSerializer>();
            services.AddSingleton<ICandlestickSerializer, CandlestickSerializer>();
            services.AddSingleton<ISymbolPriceSerializer, SymbolPriceSerializer>();
            services.AddSingleton<ISymbolStatisticsSerializer, SymbolStatisticsSerializer>();
            services.AddSingleton<IAggregateTradeSerializer, AggregateTradeSerializer>();
            services.AddSingleton<IAccountTradeSerializer, AccountTradeSerializer>();
            services.AddSingleton<ITradeSerializer, TradeSerializer>();
            services.AddSingleton<IOrderSerializer, OrderSerializer>();

            return services;
        }
    }
}
